using Microsoft.AspNetCore.Mvc.Routing;
using CoreLibModule.Utils;
using System;
using System.Collections.Generic;
using System.FastReflection;
using System.Linq;
using System.Reflection;
using InfrastructurePlugins.BaseModule.Application.Services.Attributes;
using InfrastructurePlugins.BaseModule.Application.Services.Interfaces;
using InfrastructurePlugins.BaseModule.Application.Services.Structs;
using InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces;
using ZKWeb.Web;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace InfrastructurePlugins.BaseModule.Application.Services.Bases
{
    /// <summary>
    /// 应用服务的基础类
    /// 继承这个类以后，所有公有的函数都会运行在工作单元中
    /// 并且公有函数会对应一个Api地址，格式是"/api/类名/函数名"
    /// </summary>
    public class ApplicationServiceBase :
        IController,
        IApplicationService,
        IWebsiteStartHandler
    {
        /// <summary>
        /// 当前的Http上下文
        /// </summary>
        protected virtual IHttpContext Context => HttpManager.CurrentContext;
        /// <summary>
        /// 当前的Http请求
        /// </summary>
        protected virtual IHttpRequest Request => Context.Request;
        /// <summary>
        /// 当前的Http回应
        /// </summary>
        protected virtual IHttpResponse Response => Context.Response;
        /// <summary>
        /// 获取依赖注入器(容器)
        /// </summary>
        protected virtual IContainer Injector
        {
            get { return ZKWeb.Application.Ioc; }
        }
        /// <summary>
        /// 当前使用的工作单元，设置用户的过滤器
        /// </summary>
        protected virtual IUnitOfWork UnitOfWork
        {
            get
            {
                var xUnitOfWork = Injector.Resolve<IUnitOfWork>();
                //根据当前用户，服务ID，获取工作单元相应的过滤器,规则过滤器，并应用过滤器
                //获取用户
                //获取过滤器,应用过滤器
                var userQueryFilters = Injector.Resolve<IUserQueryFilter>()?.UserQueryFilters(ServiceId);
                if (userQueryFilters != null && userQueryFilters.Count > 0) xUnitOfWork.QueryFilters = userQueryFilters;

                var userOperationFilters = Injector.Resolve<IUserOperationFilter>()?.UserOperationFilters(ServiceId);
                if (userOperationFilters != null && userOperationFilters.Count > 0) xUnitOfWork.OperationFilters = userOperationFilters;
                
                return xUnitOfWork;
            }
        }
        /// <summary>
        /// 基础地址
        /// </summary>
        protected virtual string UrlBase => $"/api/{GetType().Name}";

        private string xServiceId;
        public string ServiceId
        {
            get
            {
                if (xServiceId == null) xServiceId = MD5Utils.GetGuidStrByMD5(GetType().FullName, "X2");
                return xServiceId;
            }
        }
        /// <summary>
        /// 获取Api函数列表
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ApplicationServiceApiMethodInfo> GetApiMethods()
        {
            var serviceClassType = GetType();
            var typeInfo = serviceClassType.GetTypeInfo();
            var methods = typeInfo.GetMethods(
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            foreach (var method in methods)
            {
                if (method.IsSpecialName)
                    continue;
                if (method.ReturnType == typeof(void))
                    continue;
                // 创建函数委托
                // 如果函数未标记[UnitOfWork]则手动包装该函数
                var action = this.BuildActionDelegate(method);
                if (method.GetCustomAttribute<UnitOfWorkAttribute>() == null)
                {
                    action = new UnitOfWorkAttribute().Filter(action);
                }
                //如果函数未标记[DataSecurity]则手动包装该函数
                if (method.GetCustomAttribute<DataSecurityAttribute>() == null)
                {
                    action = new DataSecurityAttribute().Filter(action);
                }
                // 包装过滤器
                var filterAttributes = method.GetCustomAttributes<ActionFilterAttribute>();
                foreach (var filterAttribute in filterAttributes)
                {
                    action = filterAttribute.Filter(action);
                }
                //获取函数的请求方法
                var requestPath = $"{UrlBase}/{method.Name}";
                var requestMethod = HttpMethods.POST;
                var requestMethodAttributes = method.GetCustomAttributes<ActionAttribute>();
                foreach (var actionAttribute in requestMethodAttributes)
                {
                    requestMethod = actionAttribute.Method;
                    requestPath = actionAttribute.Path == "" ? requestPath : $"{UrlBase}/{actionAttribute.Path}";
                }
                // 返回函数信息
                var info = new ApplicationServiceApiMethodInfo(
                    serviceClassType,
                    method.ReturnType,
                    method.Name,
                    requestPath, requestMethod,
                    method.FastGetCustomAttributes(typeof(Attribute), true).OfType<Attribute>(),
                    method.GetParameters().Select(p => new ApplicationServiceApiParameterInfo(
                        p.ParameterType,
                        p.Name,
                        p.GetCustomAttributes(typeof(Attribute), true).OfType<Attribute>())),
                    action);
                yield return info;
            }
        }

        /// <summary>
        /// 网站启动时注册所有Api函数
        /// </summary>
        public void OnWebsiteStart()
        {
            var typeInfo = GetType().GetTypeInfo();
            var methods = typeInfo.GetMethods(
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            var controllerManager = ZKWeb.Application.Ioc.Resolve<ControllerManager>();
            foreach (var methodInfo in GetApiMethods())
            {
                var url = methodInfo.Url;
                var action = methodInfo.Action;
                controllerManager.RegisterAction(url, methodInfo.Method, action);
            }
        }
    }
}

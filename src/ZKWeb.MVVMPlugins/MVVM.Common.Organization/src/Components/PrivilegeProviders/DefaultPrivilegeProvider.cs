using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.ActionFilters;
using ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.Attributes;
using ZKWeb.Plugins.MVVM.Common.Organization.src.Components.PrivilegeProviders.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

public class TempModelInfo
{
    public Type TempClassType { get; set; }
    public IEnumerable<TempDataFieldAttribute> TempDataFields { get; set; }
}
public class TempDataFieldInfo
{
    public Type TempClassType { get; set; }
    public TempDataFieldAttribute TempDataFieldAttr { get; set; }
}

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Components.PrivilegeProviders
{
    /// <summary>
    /// 默认的权限提供器
    /// </summary>
    [ExportMany]
    public class DefaultPrivilegeProvider : IPrivilegesProvider
    {
        /// <summary>
        /// 查找应用服务中的权限并返回
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPrivileges()
        {
            var applicationServices = ZKWeb.Application.Ioc.ResolveMany<IApplicationService>();
            //功能
            var actionPrivileges = applicationServices
               .SelectMany(a => a.GetApiMethods())
               .Select(m =>
               {
                   var actionPrivilege = m.Attributes.OfType<TempActionAttribute>().First();
                   actionPrivilege.TempClassType = m.ServiceClassType;
                   return actionPrivilege;
               });

            //数据字段
            var dataFieldPrivileges = applicationServices.Select(a => new TempModelInfo()
            {
                TempClassType = a.GetType(),
                TempDataFields = a.GetType().GetTypeInfo().GetCustomAttribute<TempModelAttribute>()
                            .TemplateModels.SelectMany(t => t.GetTypeInfo().GetProperties())
                            .SelectMany(p => p.GetCustomAttributes<TempDataFieldAttribute>())
            }).SelectMany(m =>
            {
                m.TempDataFields.ForEach(t => t.TempClassType = m.TempClassType);
                return m.TempDataFields;
            });

            return applicationServices
                .SelectMany(a => a.GetApiMethods())
                .SelectMany(m => m.Attributes.OfType<CheckPrivilegeAttribute>())
                .SelectMany(a => a.RequirePrivileges)
                .Distinct();
        }
    }
}

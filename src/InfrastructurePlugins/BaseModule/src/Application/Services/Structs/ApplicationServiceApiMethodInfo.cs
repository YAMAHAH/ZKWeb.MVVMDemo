﻿using System;
using System.Collections.Generic;
using ZKWeb.Web;

namespace InfrastructurePlugins.BaseModule.Application.Services.Structs
{
    /// <summary>
    /// 应用服务中的Api函数信息
    /// </summary>
    public class ApplicationServiceApiMethodInfo
    {
        /// <summary>
        /// 功能ID
        /// 服务的fullName + 方法的名称
        /// </summary>
        public Guid ActionId { get; set; }
        /// <summary>
        /// 服务ID
        /// </summary>
        public Guid ServiceId { get; set; }
        /// <summary>
        /// 所属服务类类型
        /// </summary>
        public Type ServiceClassType { get; set; }
        /// <summary>
        /// 返回类型
        /// </summary>
        public Type ReturnType { get; set; }
        /// <summary>
        /// 函数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Url地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public string Method { get; set; } = HttpMethods.POST;
        /// <summary>
        /// 属性列表
        /// </summary>
        public IEnumerable<Attribute> Attributes { get; set; }
        /// <summary>
        /// 参数列表
        /// </summary>
        public IEnumerable<ApplicationServiceApiParameterInfo> Parameters { get; set; }
        /// <summary>
        /// Api函数主体
        /// </summary>
        public Func<IActionResult> Action { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public ApplicationServiceApiMethodInfo(Guid actionId, Guid serviceId, Type serviceClassType,
            Type returnType, string name, string url, string method,
            IEnumerable<Attribute> attributes,
            IEnumerable<ApplicationServiceApiParameterInfo> parameters,
            Func<IActionResult> action)
        {
            ActionId = actionId;
            ServiceId = serviceId;
            ServiceClassType = serviceClassType;
            ReturnType = returnType;
            Name = name;
            Url = url;
            Method = method;
            Attributes = attributes;
            Parameters = parameters;
            Action = action;
        }
    }
}

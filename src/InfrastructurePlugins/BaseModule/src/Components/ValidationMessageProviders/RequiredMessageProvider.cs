﻿using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using InfrastructurePlugins.BaseModule.Components.ValidationMessageProviders.Interfaces;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.ValidationMessageProviders
{
    /// <summary>
    /// 提供验证必填项的错误信息
    /// </summary>
    [ExportMany]
    public class RequiredMessageProvider : IValidationMessageProvider<RequiredAttribute>
    {
        public string FormatErrorMessage(RequiredAttribute attribute, string name)
        {
            return new T("{0} is required", name);
        }
    }
}

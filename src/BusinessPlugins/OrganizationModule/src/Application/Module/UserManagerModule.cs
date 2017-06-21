using System;
using System.Collections.Generic;
using System.Text;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Module;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Application.Module
{
    [ExportMany, SingletonReuse]
    [AngularModule("UserManager")]
    public class UserManagerModule : AngularModuleBase
    {
    }
}

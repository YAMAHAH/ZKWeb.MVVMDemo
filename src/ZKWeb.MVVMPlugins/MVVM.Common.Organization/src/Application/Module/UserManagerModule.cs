using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Attributes;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Organization.src.Application.Module
{
    [ExportMany, SingletonReuse]
    [AngularModule("UserManager")]
    public class UserManagerModule : AngularModuleBase
    {
    }
}

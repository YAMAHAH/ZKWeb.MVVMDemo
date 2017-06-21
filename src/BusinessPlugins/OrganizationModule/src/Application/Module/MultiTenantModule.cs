using CoreLibModule.Utils;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Module;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.OrganizationModule.Application.Module
{
    [ExportMany, SingletonReuse]
    [AngularModule("MultiTenant")]
    public class MultiTenantModule : AngularModuleBase
    {
        //一个模块对应多个模板,一个模板包含基本信息,功能,权限,过滤器
        //注册模板由父类构造函数处理,如果不使用默认的构造函数,要调用:base()
    }
}

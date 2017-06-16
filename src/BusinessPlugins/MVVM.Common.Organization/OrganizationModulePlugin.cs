using BusinessPlugins.ProductModule;
using ZKWeb.MVVMPlugins.MVVM.Common.Base;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Module;

namespace BusinessPlugins.MVVM.Common.Organization
{
    [DependsOn(typeof(BaseModulePlugin), typeof(ProductModulePlugin))]
    public class OrganizationModulePlugin : ModulePluginBase
    {
        public OrganizationModulePlugin()
        {
            EName = "MVVM.Common.Organization";
            CName = "Organization Structure For MVVM";
            Version = "1.0";
            Description = "Provide organization entities and domain services";
        }
    }
}

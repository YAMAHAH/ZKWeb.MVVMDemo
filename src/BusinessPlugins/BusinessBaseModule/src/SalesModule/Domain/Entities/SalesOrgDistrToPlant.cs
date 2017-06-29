using BusinessPlugins.OrganizationModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Entities.Bases;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 销售组织分销渠道到工厂的对多关联表
    /// </summary>
    [ExportMany]
    public class SalesOrgDistrToPlant : ManyToManyEntityBase<SalesOrgDistrChannel, Plant, SalesOrgDistrToPlant>
    {

    }
}

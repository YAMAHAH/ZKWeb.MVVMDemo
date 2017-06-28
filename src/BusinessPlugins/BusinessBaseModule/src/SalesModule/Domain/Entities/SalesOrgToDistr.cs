using InfrastructurePlugins.BaseModule.Domain.Entities.Bases;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 销售组织到分销渠道的对多关联表
    /// </summary>
    [ExportMany]
    public class SalesOrgToDistr : ManyToManyEntityBase<SalesOrganization, DistributionChannel, SalesOrgToDistr>
    {

    }
}



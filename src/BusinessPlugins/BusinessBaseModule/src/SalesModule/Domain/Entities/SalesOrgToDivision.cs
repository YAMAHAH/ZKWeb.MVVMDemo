using InfrastructurePlugins.BaseModule.Domain.Entities.Bases;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 销售组织到产品组的对多关联表
    /// </summary>
    [ExportMany]
    public class SalesOrgToDivision : ManyToManyEntityBase<SalesOrganization, SalesDivision, SalesOrgToDivision>
    {

    }
}

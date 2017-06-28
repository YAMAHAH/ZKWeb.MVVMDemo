using InfrastructurePlugins.BaseModule.Domain.Entities.Bases;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 销售区域到销售办公室的对多关联表
    /// </summary>
    [ExportMany]
    public class SalesRegionToOffice : ManyToManyEntityBase<SalesRegion, SalesOffice, SalesRegionToOffice>
    {
    }
}

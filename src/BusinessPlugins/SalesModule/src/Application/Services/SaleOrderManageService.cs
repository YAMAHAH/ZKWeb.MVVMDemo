using BusinessPlugins.SalesModule.Domain.Services;
using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Application.Services
{
    [ExportMany, SingletonReuse, Description("销售订单管理服务")]
    public class SaleOrderManageService : ApplicationServiceBase
    {
        private SaleOrderManager xSaleOrderManager;
        public SaleOrderManageService(SaleOrderManager saleOrderManager)
        {
            xSaleOrderManager = saleOrderManager;
        }

        public void CreateOrUpdateSaleOrder()
        {
            //请求数据检验，并转换成实体模型 OutpuDTO -> entity
            // var saleOrder = null;
            //
            // xSaleOrderManager.CreateOrUpdateSaleOrder(saleOrder);
        }
    }
}

using BusinessPlugins.SalesModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Services
{
    [ExportMany, SingletonReuse]
    public class SaleOrderManager : DomainServiceBase<SaleOrderHeader, Guid>
    {
        public void CreateOrUpdateSaleOrder(SaleOrderHeader saleOrder)
        {
            if (saleOrder.Id == Guid.Empty)
            {
                UnitRepository.Upsert(ref saleOrder);
            }
            else
            {
                var existEntity = UnitRepository.Query()
                    .Include(h => h.SaleOrderDetails)
                    .Where(h => h.Id == saleOrder.Id)
                    .FirstOrDefault();

                UnitRepository.UpdateMany(existEntity, saleOrder, h => h.SaleOrderDetails, mx => mx.Id, (mx1, mx2) => mx1.Id == mx2.Id);
            }
        }

        public void DeleteSaleOrder(SaleOrderHeader saleOrder)
        {
            Delete(saleOrder);
        }
    }
}

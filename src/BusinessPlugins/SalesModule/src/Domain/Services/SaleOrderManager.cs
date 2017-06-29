using BusinessPlugins.SalesModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.SalesModule.Domain.Services
{
    [ExportMany, SingletonReuse]
    public class SaleOrderManager : DomainServiceBase<SaleOrder, Guid>
    {
        public void CreateOrUpdateSaleOrder(SaleOrder saleOrder)
        {
            if (saleOrder.Id == Guid.Empty)
            {
                UnitRepository.Upsert(ref saleOrder);
            }
            else
            {
                var existEntity = UnitRepository.Query()
                    .Include(h => h.Items)
                    .Where(h => h.Id == saleOrder.Id)
                    .FirstOrDefault();

                UnitRepository.UpdateMany(existEntity, saleOrder, h => h.Items, mx => mx.Id, (mx1, mx2) => mx1.Id == mx2.Id);
            }
        }

        public void DeleteSaleOrder(SaleOrder saleOrder)
        {
            Delete(saleOrder);
        }
    }
}

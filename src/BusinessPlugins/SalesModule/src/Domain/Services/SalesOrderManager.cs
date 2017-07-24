using BusinessPlugins.SalesModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using ZKWebStandard.Ioc;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BusinessPlugins.SalesModule.Domain.Services
{
    /// <summary>
    /// 销售订单管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class SalesOrderManager : DomainServiceBase<SalesOrder, Guid>, ISalesOrderManager
    {
        /// <summary>
        /// 创建或更新销售订单
        /// </summary>
        /// <param name="saleOrder">销售订单</param>
        public void CreateOrUpdate(SalesOrder saleOrder)
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

        /// <summary>
        /// 删除指定实体的销售订单
        /// </summary>
        /// <param name="salesOrder">销售订单</param>
        public void Remove(SalesOrder salesOrder)
        {
            UnitRepository.Delete(salesOrder);
        }
        /// <summary>
        /// 获取满足条件的销售订单项
        /// </summary>
        /// <param name="predicate">销售订单项条件</param>
        /// <returns></returns>
        public IList<SalesOrderItem> SelectItems(Expression<Func<SalesOrderItem, bool>> predicate)
        {
            var SalOrdItemRepository = UnitOfWork.GetUnitRepository<SalesOrderItem, Guid>();
            return SalOrdItemRepository.QueryAsReadOnly().Where(predicate).ToList();
        }
    }
}

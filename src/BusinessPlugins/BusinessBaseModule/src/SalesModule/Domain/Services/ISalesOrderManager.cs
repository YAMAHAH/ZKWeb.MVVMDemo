using BusinessPlugins.SalesModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BusinessPlugins.SalesModule.Domain.Services
{
    /// <summary>
    /// 销售订单管理
    /// </summary>
    public interface ISalesOrderManager : IDomainService<SalesOrder, Guid>
    {
        /// <summary>
        /// 创建或更新销售订单
        /// </summary>
        /// <param name="salesOrder"></param>
        void CreateOrUpdate(SalesOrder salesOrder);
        /// <summary>
        /// 删除指定销售订单
        /// </summary>
        /// <param name="salesOrder">销售订单</param>
        void Remove(SalesOrder salesOrder);
        /// <summary>
        /// 获取满足条件的销售订单项
        /// </summary>
        /// <param name="predicate">销售订单项条件</param>
        /// <returns></returns>
        IList<SalesOrderItem> SelectItems(Expression<Func<SalesOrderItem, bool>> predicate);

        
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.SalesModule.Domain.Entities
{
    /// <summary>
    /// 销售订单类型
    /// </summary>
    public enum SalesOrderType
    {
        /// <summary>
        /// 零售订单
        /// </summary>
        RetailOrder = 0,
        /// <summary>
        /// 需求订单
        /// 根据订单生产
        /// </summary>
        MakeToOrder = 1,
        /// <summary>
        /// 库存订单
        /// 订单消耗预测库存
        /// </summary>
        MakeToStock =2,
        /// <summary>
        /// 装配订单
        /// 根据客户要求选配生产
        /// </summary>
        AssembleToOrder = 3,
        /// <summary>
        /// 免费订单(DFOC)
        /// </summary>
        DelivFreeofCharge = 4,
        /// <summary>
        /// 退货销售订单
        /// </summary>
        ReturnOrder = 5


    }
}

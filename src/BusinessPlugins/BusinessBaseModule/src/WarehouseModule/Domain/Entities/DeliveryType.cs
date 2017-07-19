using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 交货类型
    /// </summary>
    public enum DeliveryType
    {
        /// <summary>
        /// 销售交货
        /// </summary>
        SalesDelivery = 0,
        /// <summary>
        /// 采购交货
        /// </summary>
        PurchaseDelivery = 1
    }
}

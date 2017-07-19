using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 库存类型
    /// 公司库存S、供应商寄售库存K、寄存在客户的库存C、供给供应商/分包商的库存V、按业务分配的销售订单库存与项目库存
    /// </summary>
    public enum InventoryType
    {
        /// <summary>
        /// 公司库存
        /// </summary>
        CompanyStock = 0,
        /// <summary>
        /// 销售订单
        /// </summary>
        SalesOrder = 1,
        /// <summary>
        /// 项目库存
        /// </summary>
        Project = 2,
        /// <summary>
        /// 供应商
        /// </summary>
        Vendor = 3,
        /// <summary>
        /// 客户
        /// </summary>
        Customer = 4,
        /// <summary>
        /// 供应商寄售 
        /// 供应商把物料放在企业里,归属权属于供应商
        /// </summary>
        VendorConsignment = 5,
        /// <summary>
        /// 客户寄售
        /// 企业把物料放在客户哪里,归属权属于企业
        /// </summary>
        CustomerConsignment = 6
    }
}

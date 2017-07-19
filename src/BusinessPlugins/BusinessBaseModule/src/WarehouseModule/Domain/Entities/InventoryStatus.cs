using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.WarehouseModule.Domain.Entities
{
    /// <summary>
    /// 库存类型
    /// 公司库存S、供应商寄售库存K、寄存在客户的库存C、供给供应商/分包商的库存V、按业务分配的销售订单库存与项目库存
    /// Quality inspection
    /// 库存状态有：安全库存、在途库存、冻结库存、质检库存、非限制使用库存等。
    /// 非限制库存、质检库存、冻结库存、退货库存、转移中的库存、在途库存、限制使用的库存
    /// </summary>
    public enum InventoryStatus
    {
        /// <summary>
        /// 非限制
        /// </summary>
        UnRestricted = 0,
        /// <summary>
        /// 限制
        /// </summary>
        Restrict = 1,
        /// <summary>
        /// 冻结
        /// </summary>
        Freeze = 2,
        /// <summary>
        /// 质检
        /// </summary>
        QualityInspection = 3,
        /// <summary>
        /// 退货
        /// </summary>
        Return = 4,
        /// <summary>
        /// 转移
        /// </summary>
        Transfer = 5,
        /// <summary>
        /// 在途
        /// </summary>
        Transit = 6,
        /// <summary>
        /// 安全库存
        /// </summary>
        SafelyStock = 7
    }
}

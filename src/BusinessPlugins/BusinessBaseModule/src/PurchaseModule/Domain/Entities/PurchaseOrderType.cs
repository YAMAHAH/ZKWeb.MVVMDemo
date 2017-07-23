namespace BusinessPlugins.PurchaseModule.Domain.Entities
{
    /// <summary>
    /// 采购订单类型
    /// </summary>
    public enum PurchaseOrderType
    {
        /// <summary>
        /// 标准采购订单
        /// </summary>
        StandardPurchaseOrder = 0,
        /// <summary>
        /// 退货采购订单
        /// </summary>
        ReturnPurchaseOrder = 1,
        /// <summary>
        /// 免费采购订单
        /// </summary>
        FreePurchaseOrder = 2,
        /// <summary>
        /// 外包采购订单
        /// </summary>
        SubcontractPurchaseOrder = 3,
        /// <summary>
        /// 客供料采购订单
        /// 客户提供的物料收到客户库存，生产订单发料消耗客户库存
        /// </summary>
        ConsignmentPurchaseOrder = 4
    }
}

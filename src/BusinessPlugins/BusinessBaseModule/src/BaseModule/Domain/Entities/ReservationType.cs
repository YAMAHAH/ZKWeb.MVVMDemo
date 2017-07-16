namespace BusinessPlugins.BaseModule.Domain.Entities
{
    /// <summary>
    /// 预留类型
    /// </summary>
    public enum ReservationType
    {
        /// <summary>
        /// 销售订单
        /// </summary>
        SalesOrder = 0,
        /// <summary>
        /// 计划采购订单
        /// </summary>
        PlanPurchaseOrder = 1,
        /// <summary>
        /// 计划生产订单
        /// </summary>
        PlanProductionOrder = 2,
        /// <summary>
        /// 采购订单
        /// </summary>
        PurchaseOrder = 3,
        /// <summary>
        /// 生产订单
        /// </summary>
        ProductionOrder = 4
    }
}

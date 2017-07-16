namespace BusinessPlugins.BaseModule.Domain.Entities
{
    public enum PurchaseType
    {
        /// <summary>
        /// 标准采购
        /// </summary>
        StandandPurchase = 0,
        /// <summary>
        /// 客供料
        /// </summary>
        ConsignMaterial = 1,
        /// <summary>
        /// 物料外包
        /// </summary>
        MaterialSubcontract = 2,
        /// <summary>
        /// 工序外协
        /// </summary>
        ProcessSubcontract = 3
    }
}

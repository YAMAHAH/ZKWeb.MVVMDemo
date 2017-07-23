namespace BusinessPlugins.ProductionModule.Domain.Entities
{
    /// <summary>
    /// 制造订单类型
    /// </summary>
    public enum ManufactureOrderType
    {
        /// <summary>
        /// 标准制造订单
        /// </summary>
        StandardManufactureOrder = 0,
        /// <summary>
        /// 返工制造订单
        /// </summary>
        ReturnManufactureOrder = 1,
        /// <summary>
        /// 拆解订单
        /// </summary>
        DisassemblyOrder = 2
    }
}

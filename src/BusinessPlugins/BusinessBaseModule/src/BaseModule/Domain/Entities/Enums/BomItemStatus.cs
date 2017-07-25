namespace BusinessPlugins.BaseModule.Domain.Entities
{
    /// <summary>
    /// BOM行状态
    /// </summary>
    public enum BomItemStatus
    {
        unConfirmed = 0,//待确认
        Ok = 1, //确认
        Cancel = 2 //取消
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.BaseModule.Domain.Entities
{
    public enum BomItemStatus
    {
        unConfirmed = 0,//待确认
        Ok = 1, //确认
        Cancel = 2 //取消
    }
}

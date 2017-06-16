﻿using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.Domain.Entities.Interfaces
{
    /// <summary>
    ///指示实体的状态
    /// </summary>
    /// <typeparam name="TStatus"></typeparam>
    public interface IHaveEntityStatus<TStatus> : IEntity where TStatus : class
    {
        TStatus Status { get; set; }
    }
}

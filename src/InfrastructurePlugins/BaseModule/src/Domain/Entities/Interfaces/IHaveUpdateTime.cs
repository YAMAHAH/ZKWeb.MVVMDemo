﻿using System;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces
{
    /// <summary>
    /// 包含更新时间的接口
    /// </summary>
    public interface IHaveUpdateTime : IEntity
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime UpdateTime { get; set; }
    }
}
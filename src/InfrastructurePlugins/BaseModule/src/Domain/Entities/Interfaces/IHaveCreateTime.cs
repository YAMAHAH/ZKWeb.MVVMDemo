using System;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces
{
    /// <summary>
    /// 包含创建时间的接口
    /// </summary>
    public interface IHaveCreateTime : IEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}

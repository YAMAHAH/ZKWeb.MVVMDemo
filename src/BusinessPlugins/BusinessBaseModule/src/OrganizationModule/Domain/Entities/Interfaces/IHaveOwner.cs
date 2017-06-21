﻿using System;
using ZKWeb.Database;

namespace BusinessPlugins.OrganizationModule.Domain.Entities.Interfaces
{
    /// <summary>
    /// 包含所属的用户
    /// </summary>
    public interface IHaveOwner : IEntity
    {
        /// <summary>
        /// 所属的用户
        /// </summary>
        User Owner { get; set; }
        /// <summary>
        /// 所属的用户Id
        /// </summary>
        Guid OwnerId { get; set; }
    }
}

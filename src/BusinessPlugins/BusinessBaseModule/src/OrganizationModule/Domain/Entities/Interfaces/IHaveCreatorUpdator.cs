using BusinessPlugins.OrganizationModule.Domain.Entities;
using System;
using ZKWeb.Database;

namespace BusinessPlugins.OrganizationModule.Domain.Entities.Interfaces
{
    public interface IHaveCreatorUpdator : IEntity
    {
        /// <summary>
        /// 新增的用户
        /// </summary>
        Employee Creator { get; set; }
        /// <summary>
        /// 新增的用户Id
        /// </summary>
        Guid CreatorId { get; set; }
        /// <summary>
        /// 更新的用户
        /// </summary>
        Employee Updator { get; set; }
        /// <summary>
        /// 更新的用户Id
        /// </summary>
        Guid UpdatorId { get; set; }
    }
}

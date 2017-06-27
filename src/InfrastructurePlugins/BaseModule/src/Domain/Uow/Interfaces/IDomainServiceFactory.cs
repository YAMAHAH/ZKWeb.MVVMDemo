using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;

namespace InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces
{
    public interface IDomainServiceFactory
    {
        /// <summary>
        ///获取通用的服务类型
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">主键类型</typeparam>
        /// <returns></returns>
        DomainServiceBase<TEntity, TPrimaryKey> GetDomainService<TEntity, TPrimaryKey>()
            where TEntity : class, IEntity<TPrimaryKey>, new();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Domain.Services.Bases
{
    public class GenericService<TEntity, TPrimaryKey> : DomainServiceBase<TEntity, TPrimaryKey>
         where TEntity : class, IEntity<TPrimaryKey>, new()
    {
    }
}

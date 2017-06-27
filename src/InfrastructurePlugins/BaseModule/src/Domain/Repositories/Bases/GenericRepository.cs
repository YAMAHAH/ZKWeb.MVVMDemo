using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Domain.Repositories.Bases
{
    public class GenericRepository<TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>
         where TEntity : class, IEntity<TPrimaryKey>
    {

    }
}

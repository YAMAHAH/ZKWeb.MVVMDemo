using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Repositories.Bases
{
    public class GenericRepository<TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>
         where TEntity : class, IEntity<TPrimaryKey>
    {

    }
}

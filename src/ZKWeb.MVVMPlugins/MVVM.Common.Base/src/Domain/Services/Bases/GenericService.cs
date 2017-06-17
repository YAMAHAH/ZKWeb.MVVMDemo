using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases
{
    public class GenericService<TEntity, TPrimaryKey> : DomainServiceBase<TEntity, TPrimaryKey>
         where TEntity : class, IEntity<TPrimaryKey>, new()
    {
    }
}

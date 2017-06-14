using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces
{
    public interface IHaveChildren<TEntity, TPrimaryKey> : IEntity
    {
        List<TEntity> Childs { get; set; }
    }
}

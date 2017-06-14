using System;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces
{
    public interface IHaveTreeNode<TEntity, TPrimaryKey> : IEntity where TPrimaryKey : struct
    {
        TPrimaryKey RootId { get; set; }
        Nullable<TPrimaryKey> ParentId { get; set; }
        TEntity Parent { get; set; }
    }
}

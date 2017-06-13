using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces
{
   public interface IHaveTreeNode<TPrimaryKey>:IEntity
    {
        TPrimaryKey RootId { get; set; }
        TPrimaryKey ParentId { get; set; }
        IHaveTreeNode<TPrimaryKey> Parent { get; set; }
    }
}

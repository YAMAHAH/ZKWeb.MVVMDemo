using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.Domain.Entities.Interfaces
{
    public interface ITreeStructType<TEntity, TPrimaryKey> : IHaveChildren<TEntity, TPrimaryKey>,
         IHaveTreeNode<TEntity, TPrimaryKey> where TPrimaryKey:struct
    {
    }
}

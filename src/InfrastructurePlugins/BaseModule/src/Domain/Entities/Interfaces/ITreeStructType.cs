using System;
using System.Collections.Generic;
using System.Text;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;

namespace InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces
{
    public interface ITreeStructType<TEntity, TPrimaryKey> : IHaveChildren<TEntity, TPrimaryKey>,
         IHaveTreeNode<TEntity, TPrimaryKey> where TPrimaryKey:struct
    {
    }
}

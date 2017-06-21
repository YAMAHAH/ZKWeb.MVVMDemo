using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces
{
    public interface IHaveChildren<TEntity, TPrimaryKey> : IEntity
    {
        List<TEntity> Childs { get; set; }
    }
}

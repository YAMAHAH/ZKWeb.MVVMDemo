using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces
{
    public interface IHaveNodeVersion<TPrimaryKey> : IEntity
    {
        TPrimaryKey NodeVersionId { get; set; }
        IHaveNodeVersion<TPrimaryKey> NodeVersion { get; set; }
    }
}

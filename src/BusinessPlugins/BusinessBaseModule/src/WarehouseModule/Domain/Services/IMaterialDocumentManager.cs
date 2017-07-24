using BusinessPlugins.WarehouseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.WarehouseModule.Domain.Services
{
    /// <summary>
    /// 物料凭证管理
    /// </summary>
    public interface IMaterialDocumentManager : IDomainService<MaterialDocument, Guid>
    {
    }
}

using BusinessPlugins.ProductEngineeringModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessPlugins.ProductEngineeringModule.Domain.Services
{
    /// <summary>
    /// 产品管理
    /// </summary>
    public interface IProductManager : IDomainService<Product, Guid>
    {
    }
}

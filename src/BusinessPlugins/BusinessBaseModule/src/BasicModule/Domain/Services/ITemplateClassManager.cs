using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 模板类管理
    /// </summary>
    public interface ITemplateClassManager : IDomainService<TemplateClass, Guid>
    {
        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="tempClsEntities"></param>
        void BatchSave(IEnumerable<TemplateClass> tempClsEntities);
    }
}

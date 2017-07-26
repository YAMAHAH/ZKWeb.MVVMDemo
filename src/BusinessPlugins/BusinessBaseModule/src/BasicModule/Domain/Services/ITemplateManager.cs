using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    public interface ITemplateManager : IDomainService<Template, Guid>
    {
        /// <summary>
        /// 获取某个模板的对象
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Dictionary<Guid, TemplateObject> GetTemplateObjects(Guid templateId);
        /// <summary>
        /// 获取某个用户某个模板的权限对象
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Dictionary<Guid, TemplateObject> GetTemplateObjects(Guid employeeId, Guid templateId);
    }
}

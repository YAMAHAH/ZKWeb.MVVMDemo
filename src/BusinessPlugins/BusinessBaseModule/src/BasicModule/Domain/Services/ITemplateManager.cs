using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    public interface ITemplateManager : IDomainService<Template, Guid>
    {
        /// <summary>
        /// 获取某个模板的对象字典
        /// </summary>
        /// <param name="tempId"></param>
        /// <returns></returns>
        Dictionary<Guid, TemplateObject> GetTemplateObjects(Guid tempId);    
    }
}

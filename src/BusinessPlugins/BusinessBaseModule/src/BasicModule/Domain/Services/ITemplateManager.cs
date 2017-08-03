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
        /// <summary>
        /// 获取多个模板的对象字典
        /// </summary>
        /// <param name="tempIds"></param>
        /// <returns></returns>
        Dictionary<string, Dictionary<Guid, TemplateObject>> GetTemplateObjects(IList<KeyValuePair<Guid, Guid>> userTempIds);
        /// <summary>
        /// 获取某个模板某个对象
        /// </summary>
        /// <param name="tempId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        TemplateObject GetTemplateObject(Guid tempId, Guid objectId);
    }
}

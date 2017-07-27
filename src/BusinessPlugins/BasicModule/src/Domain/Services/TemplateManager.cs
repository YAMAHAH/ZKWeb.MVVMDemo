using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Entities;
using BusinessPlugins.OrganizationModule.Domain.Services;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 模板管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class TemplateManager : DomainServiceBase<Template, Guid>, ITemplateManager
    {
        public Dictionary<Guid, TemplateObject> GetTemplateObjects(Guid templateId)
        {
            var tempObjRepository = UnitOfWork.GetUnitRepository<TemplateObject, Guid>();

            return tempObjRepository.FastQueryAsReadOnly()
                .Where(to => to.TempId == templateId)
                .Include(t => t.Template)
                .ToDictionary(t => t.Id);
        }
    }
}

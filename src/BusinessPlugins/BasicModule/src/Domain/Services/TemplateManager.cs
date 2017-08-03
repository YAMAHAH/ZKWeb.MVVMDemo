using BusinessPlugins.BasicModule.Domain.Entities;
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
        public TemplateObject GetTemplateObject(Guid tempId, Guid objectId)
        {
            var tempObjRep = UnitOfWork.GetUnitRepository<TemplateObject, Guid>();

            return tempObjRep.FastQueryAsReadOnly()
                .Where(t => t.TempId == tempId && t.Id == objectId)
                .FirstOrDefault();
        }

        public Dictionary<Guid, TemplateObject> GetTemplateObjects(Guid tempId)
        {
            var tempObjRep = UnitOfWork.GetUnitRepository<TemplateObject, Guid>();

            return tempObjRep.FastQueryAsReadOnly()
                .Where(to => to.TempId == tempId)
                .Include(t => t.Template)
                .ToDictionary(t => t.Id);
        }

        public Dictionary<string, Dictionary<Guid, TemplateObject>> GetTemplateObjects(IList<KeyValuePair<Guid, Guid>> userTempIds)
        {
            var tempObjRep = UnitOfWork.GetUnitRepository<TemplateObject, Guid>();

            var tempIds = userTempIds.Select(t => t.Value).ToList();

            var temp = tempObjRep.FastQueryAsReadOnly()
                .Where(t => tempIds.Contains(t.TempId))
                .Include(t => t.Template)
                .GroupBy(t => t.TempId)
                .Select(t => new { key = t.Key, tempObjs = t.ToList() })
                .ToDictionary(t => t.key, t => t.tempObjs);

            var result = userTempIds
                .Select(k => new { userId = k.Key, tempId = k.Value })
                .Join(temp, a => a.tempId, t => t.Key, (a, k) => new { userId = a.userId, tempId = a.tempId, tempObjs = k.Value.ToDictionary(t => t.Id) })
                .ToDictionary(r => r.userId.ToString() + "_" + r.tempId.ToString(), k => k.tempObjs);

            return result;

            //var temp tempObjRep.FastQueryAsReadOnly()
            //    .Where(t => tempIds.Contains(t.TempId))
            //    .Include(t => t.Template)
            //    .GroupBy(t => t.TempId)
            //    .Select(t => new { key = t.Key, tempObjs = t.ToList() })
            //    .ToDictionary(t => t.key, t => t.tempObjs);
        }
    }
}

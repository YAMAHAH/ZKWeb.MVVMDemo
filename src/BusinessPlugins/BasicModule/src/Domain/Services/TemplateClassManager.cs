using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 模板类管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class TemplateClassManager : DomainServiceBase<TemplateClass, Guid>, ITemplateClassManager
    {
        public void BatchSave(IEnumerable<TemplateClass> tempClsEntities)
        {
            var unitRep = UnitRepository;
            //获取工作单元管理
            var uowMan = Injector.Resolve<IUnitOfWorkManager>();
            //开启事务
            using (var uow = uowMan.Begin())
            {
                //获取更新实体的ID集
                var tempClsIds = tempClsEntities.Select(t => t.Id).ToList();
                //获取已经存在的实体
                var existTempClses = unitRep.FastQuery()
                     .Include(t => t.ClassObjects)
                     .Where(t => tempClsIds.Contains(t.Id));

                //实体差异比较
                var entityDiff = unitRep.GetEntityDiffer(existTempClses, tempClsEntities, t => t.Id);
                //实体删除
                entityDiff.DeletedEntities.ForEach(delEntity => unitRep.UpdateState(delEntity, EntityState.Deleted));
                //实体新增
                unitRep.Insert(entityDiff.AddedEntities.ToArray());
                //实体修改
                foreach (var modEntity in entityDiff.ModifiedEntities)
                {
                    var oldEntity = existTempClses.Where(extEntity => modEntity.Id == extEntity.Id).FirstOrDefault();
                    unitRep.UpdateMany(oldEntity, modEntity, t => t.ClassObjects, t => t.Id, (exist, now) => exist.Id == now.Id);
                }
                //保存并提交事务
                uow.Complete();
            }
        }
    }
}

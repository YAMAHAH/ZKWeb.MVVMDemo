using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.BasicModule.Domain.Services;
using BusinessPlugins.BasicModule.ModuleCatalogs;
using CoreLibModule.Utils;
using InfrastructurePlugins.BaseModule.Module;
using InfrastructurePlugins.BaseModule.ModuleCatalogs;
using System;
using System.Linq;
using ZKWeb.Plugin;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule
{
    /// <summary>
    /// 插件载入时的处理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class Plugin : IPlugin
    {
        /// <summary>
        /// 插件载入时处理
        /// </summary>
        public Plugin()
        {
            //获取DI注入器
            var injector = ZKWeb.Application.Ioc;
            //获取所有的模块
            var modules = injector.ResolveMany<IModuleCatalog>();
            //生成相应的模块实体
            var moduleEntities = modules.OfType<ModuleCatalogBase>()
                .Select(m => new ModuleCatalog()
                {
                    Id = m.ModuleId,
                    ModuleCode = m.ModuleCode,
                    ModuleName = m.ModuleName,
                    Remark = m.ModuleRemark,
                    OwnerTenantId = Guid.Parse("0083cd30-dd58-102a-77e2-a921c8e556bf")
                });
            //获取模块仓储
            var moduleManRep = injector.Resolve<IModuleCatalogManager>();
            //保存数据
            moduleManRep.BatchSave(ref moduleEntities);

            //获取模板信息
            var moduleMan = injector.Resolve<ModuleManager>();
            //获取模板类信息
            var tempClsInfos = moduleMan.GetModuleComponentClassInfos();
            //生成相应的模板类实体
            var tempClsEntities = tempClsInfos.Select(t => new TemplateClass()
            {
                Id = Guid.Parse(t.TempId),
                ModuleId = Guid.Parse(t.ModuleCatalogId),
                Name = t.TempName,
                OwnerTenantId = Guid.Parse("0083cd30-dd58-102a-77e2-a921c8e556bf"),
                Title = t.TempTitle,
                ClassObjects = t.TempActions.Select(a => new TemplateClassObject()
                {
                    Id = MD5Utils.GetGuidByMD5(t.TempClassType.FullName + a.Name, "X2"),
                    TempClsId = Guid.Parse(t.TempId),
                    ObjectName = a.Name,
                    ObjectAlias = a.Name,
                    ObjectTitle = a.Text,
                    Enable = a.Enable,
                    Visible = a.Enable,
                    ObjectType = TemplateObjectType.Action,
                    OwnerTenantId = Guid.Parse("0083cd30-dd58-102a-77e2-a921c8e556bf")
                })
                .Union(t.TempDataFields.Select(a => new TemplateClassObject()
                {
                    Id = MD5Utils.GetGuidByMD5(t.TempClassType.FullName + a.Name, "X2"),
                    TempClsId = Guid.Parse(t.TempId),
                    ObjectName = a.Name,
                    ObjectTitle = a.Text,
                    Visible = a.Visible,
                    Editable = a.Editable,
                    Queryable = a.Queryable,
                    Required = a.required,
                    ObjectAlias = a.Alias,
                    ObjectType = TemplateObjectType.DataField,
                    OwnerTenantId = Guid.Parse("0083cd30-dd58-102a-77e2-a921c8e556bf")
                }))
                .ToList()
            });
            //获取模板类服务
            var tempClsMan = injector.Resolve<ITemplateClassManager>();
            //保存数据
            tempClsMan.BatchSave(ref tempClsEntities);


        }
    }
}
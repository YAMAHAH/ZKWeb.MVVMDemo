using BusinessPlugins.BasicModule.Domain.Entities;
using BusinessPlugins.BasicModule.Domain.Services;
using BusinessPlugins.BasicModule.ModuleCatalogs;
using CoreLibModule.Utils;
using InfrastructurePlugins.BaseModule.Module;
using InfrastructurePlugins.BaseModule.ModuleCatalogs;
using InfrastructurePlugins.MultiTenantModule.Domain.Services;
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
            //获取主租户
            var tenantMan = injector.Resolve<TenantManager>();
            var ownerTenantId = tenantMan.EnsureMasterTenant().Id;
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
                    OwnerTenantId = ownerTenantId
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
                Id = t.TempId,
                ModuleId = t.ModuleCatalogId,
                Name = t.TempName,
                OwnerTenantId = ownerTenantId,
                Title = t.TempTitle,
                ClassObjects = t.TempActions.Select(a => new TemplateClassObject()
                {
                    Id = MD5Utils.GetGuidByMD5(t.TempClassType.FullName + a.ActionName),
                    TempClsId = t.TempId,
                    ObjectName = a.Name,
                    ObjectAlias = a.ActionName,
                    ObjectGroup = t.TempClassType.Name.Replace("Service", ""),
                    ObjectTitle = a.Text,
                    Enable = a.Enable,
                    Visible = a.Enable,
                    ObjectType = TemplateObjectType.Action,
                    OwnerTenantId = ownerTenantId
                }).Union(t.TempFilters.Select(f => new TemplateClassObject()
                {
                    Id = MD5Utils.GetGuidByMD5(t.TempClassType.FullName + f.FilterType.Name),
                    TempClsId = t.TempId,
                    ObjectName = f.FilterType.Name,
                    ObjectAlias = f.FilterType.FullName,
                    ObjectGroup = t.TempClassType.Name.Replace("Service", ""),
                    ObjectTitle = f.FilterText,
                    Enable = true,
                    Visible = true,
                    ObjectType = TemplateObjectType.Filter,
                    OwnerTenantId = ownerTenantId
                }))
                .Union(t.TempDataFields.Select(a => new TemplateClassObject()
                {
                    Id = MD5Utils.GetGuidByMD5(t.TempClassType.FullName + a.Alias),
                    TempClsId = t.TempId,
                    ObjectGroup = a.GroupType.Name.Replace("OutputDto", ""),
                    ObjectName = a.Name,
                    ObjectAlias = a.Alias,
                    DataType = a.DataType,
                    ObjectTitle = a.Text,
                    Visible = a.Visible,
                    Editable = a.Editable,
                    Queryable = a.Queryable,
                    Required = a.required,
                    ObjectType = TemplateObjectType.DataField,
                    OwnerTenantId = ownerTenantId
                })).ToList()
            });
            //获取模板类服务
            var tempClsMan = injector.Resolve<ITemplateClassManager>();
            //保存数据
            tempClsMan.BatchSave(tempClsEntities);
        }
    }
}
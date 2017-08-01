using BusinessPlugins.BasicModule.Application.Module;
using BusinessPlugins.BasicModule.Application.Services;
using BusinessPlugins.BasicModule.ModuleCatalogs;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Domain.Filters.Interfaces;
using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace BusinessPlugins.BasicModule.Application.Filters
{
    /// <summary>
    /// 自动设置Guid主键值
    /// </summary>
    [ExportMany]
    [ComponentFilter(typeof(BasicModuleCatalog), typeof(GlobalManagerModule), typeof(GlobalManageService), "自动设置主键值")]
    public class GuidEntityFilter : IEntityOperationFilter
    {
        /// <summary>
        /// 自动设置Guid主键值
        /// </summary>
        void IEntityOperationFilter.FilterSave<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (typeof(TPrimaryKey) == typeof(Guid))
            {
                var eg = (IEntity<Guid>)entity;
                if (eg.Id == Guid.Empty)
                {
                    // 主键是空时自动生成主键
                    eg.Id = GuidUtils.SequentialGuid(DateTime.UtcNow);
                }
            }
        }

        /// <summary>
        /// 不需要处理删除
        /// </summary>
        void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity) { }
    }
}

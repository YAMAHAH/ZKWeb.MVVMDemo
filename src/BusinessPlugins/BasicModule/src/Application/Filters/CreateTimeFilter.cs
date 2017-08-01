using BusinessPlugins.BasicModule.Application.Module;
using BusinessPlugins.BasicModule.Application.Services;
using BusinessPlugins.BasicModule.ModuleCatalogs;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Domain.Entities.Interfaces;
using InfrastructurePlugins.BaseModule.Domain.Filters.Interfaces;
using System;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Application.Filters
{
    /// <summary>
    /// 自动设置实体的创建时间
    /// </summary>
    [ExportMany]
    [ComponentFilter(typeof(BasicModuleCatalog), typeof(GlobalManagerModule), typeof(GlobalManageService), "自动设置实体的创建时间")]
    public class CreateTimeFilter : IEntityOperationFilter
    {
        /// <summary>
        /// 自动设置实体的创建时间
        /// </summary>
        void IEntityOperationFilter.FilterSave<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (entity is IHaveCreateTime)
            {
                var et = (IHaveCreateTime)entity;
                if (et.CreateTime == default(DateTime))
                {
                    et.CreateTime = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// 不需要处理删除
        /// </summary>
        void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity) { }
    }
}

using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Components.GridSearchResponseBuilder;
using System;
using System.Linq.Expressions;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Components.DtoToModelMap
{
    public class DtoToModelMapValue<TModel, TPrimaryKey> : IDtoToModelMapValueBase
         where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// 列类型
        /// </summary>
        public Type ColumnType { get; set; }
        /// <summary>
        /// 列对应的表达式
        /// </summary>
        public LambdaExpression Expression { get; set; }

        public QueryColumnFilterDelegate<TModel, TPrimaryKey> ColumnFilter { get; set; }
        /// <summary>
        /// 自定义列委托,返回表达式树
        /// </summary>
        public ColumnFilterWrapperDelegate<TModel, TPrimaryKey> ColumnFilterWrapper { get; set; }
        /// <summary>
        /// 是否自定义委托
        /// </summary>
        public bool IsCustomColumnFilter { get; set; }
        public QueryColumnFilterFunc<TModel, TPrimaryKey> ColumnFilterFunc { get; set; }
        /// <summary>
        /// 模板类对象字典信息
        /// </summary>
        public ComponentPropertyAttribute TemplateObjectInfo { get; set; }
    }
}

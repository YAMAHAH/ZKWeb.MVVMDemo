using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Components.GridSearchResponseBuilder;
using InfrastructurePlugins.BaseModule.Module;
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
        /// 列所属对象的类型
        /// </summary>
        public Type ObjectType { get; set; }
        /// <summary>
        /// 属性归类
        /// </summary>
        public PropClassify PropertyClassify { get; set; }

        /// <summary>
        /// 前缀
        /// 例:e.a.b.c 前缀:a.b 列为c
        /// 前缀根据对象结构自动生成
        /// </summary>
        public string Prefix { get; set; }
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

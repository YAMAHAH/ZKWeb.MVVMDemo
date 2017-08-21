using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Components.GridSearchResponseBuilder;
using System;
using System.Linq.Expressions;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Components.DtoToModelMap
{

    /// <summary>
    /// 配置选项
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class DtoToModelMapOption<TModel, TPrimaryKey> where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        public LambdaExpression Expression { get; set; }

        public QueryColumnFilterDelegate<TModel, TPrimaryKey> ColumnFilter { get; set; }

        public ColumnFilterWrapperDelegate<TModel, TPrimaryKey> ColumnFilterWrapper { get; set; }

        public QueryColumnFilterFunc<TModel, TPrimaryKey> ColumnFilterFunc { get; set; }

        public ComponentPropertyAttribute TemplateClassObjectInfo { get; set; } = new ComponentPropertyAttribute();
        public Action<ComponentPropertyAttribute> ObjectInfoAction { get; set; }

        /// <summary>
        /// 映射到Model的表达式树
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public DtoToModelMapOption<TModel, TPrimaryKey> Map<TMember>(Expression<Func<TModel, TMember>> expression)
        {
            this.Expression = expression;
            return this;
        }
        /// <summary>
        /// 暂时不用
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public DtoToModelMapOption<TModel, TPrimaryKey> Map(Expression<Func<TModel, GridSearchColumnFilter, bool>> expression)
        {
            this.Expression = expression;
            return this;
        }
        /// <summary>
        /// 暂时不用
        /// </summary>
        /// <param name="columnFilter"></param>
        /// <returns></returns>
        public DtoToModelMapOption<TModel, TPrimaryKey> MapColumnFilter(QueryColumnFilterDelegate<TModel, TPrimaryKey> columnFilter)
        {
            ColumnFilter = columnFilter;
            return this;
        }
        /// <summary>
        /// 映射列过滤包装函数
        /// </summary>
        /// <param name="columnFilterWrapper"></param>
        /// <returns></returns>
        public DtoToModelMapOption<TModel, TPrimaryKey> MapColumnFilterWrapper(ColumnFilterWrapperDelegate<TModel, TPrimaryKey> columnFilterWrapper)
        {
            ColumnFilterWrapper = columnFilterWrapper;
            return this;
        }
        /// <summary>
        /// 配置字典信息
        /// </summary>
        /// <param name="objectInfoAction">字典信息</param>
        /// <param name="parent">所属的父结点</param>
        /// <returns></returns>
        public DtoToModelMapOption<TModel, TPrimaryKey> MapObjectDictInfo(Action<ComponentPropertyAttribute> objectInfoAction)
        {
            ObjectInfoAction = objectInfoAction;
            objectInfoAction(TemplateClassObjectInfo);
            return this;
        }
        /// <summary>
        /// 创建一个组对象
        /// </summary>
        /// <param name="GroupName">组名称</param>
        /// <param name="parent">父亲结点,应该指定为某个组名</param>
        /// <returns></returns>
        public DtoToModelMapOption<TModel, TPrimaryKey> CreateGroup(string GroupName, string text, string parent = null)
        {
            TemplateClassObjectInfo.Name = GroupName;
            TemplateClassObjectInfo.Text = text;
            TemplateClassObjectInfo.Parent = parent;
            return this;
        }
        /// <summary>
        /// 暂时不用
        /// </summary>
        /// <param name="columnFilterFunc"></param>
        /// <returns></returns>
        public DtoToModelMapOption<TModel, TPrimaryKey> MapFunc(QueryColumnFilterFunc<TModel, TPrimaryKey> columnFilterFunc)
        {
            ColumnFilterFunc = columnFilterFunc;
            return this;
        }
    }
}

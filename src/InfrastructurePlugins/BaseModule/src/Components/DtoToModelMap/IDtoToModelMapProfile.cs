using InfrastructurePlugins.BaseModule.Application.Dtos;
using System;
using System.Linq.Expressions;
using ZKWeb.Database;

namespace InfrastructurePlugins.BaseModule.Components.DtoToModelMap
{
    public interface IDtoToModelMapProfile<TModel, TDto, TPrimaryKey> : IDtoToModelMapProfileBase
       where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 获取outputDto的关键字过滤字段选择表达式树
        /// </summary>
        Expression<Func<TModel, object>> KeywordFilterExpression { get; set; }
        /// <summary>
        /// 隶属于模板类的类型
        /// </summary>
        Type TemplateClassType { get; set; }
        /// <summary>
        /// 创建新实例
        /// </summary>
        /// <param name="column"></param>
        /// <param name="columnType"></param>
        /// <returns></returns>
        DtoToModelMapValue<TModel, TPrimaryKey> CreateMapValue(string column, Type columnType, LambdaExpression expression);
        /// <summary>
        /// 添加新值
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        void AddOrUpdate(string propName, DtoToModelMapValue<TModel, TPrimaryKey> value);
        /// <summary>
        /// 配置outputDTO和Model字段的对应关系
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="destMember"></param>
        /// <param name="optionAction"></param>
        /// <returns></returns>
        DtoToModelMapProfile<TModel, TDto, TPrimaryKey> ForMember<TMember>(Expression<Func<TDto, TMember>> destMember,
        Action<DtoToModelMapOption<TModel, TPrimaryKey>> optionAction);
        /// <summary>
        /// 创建新成员,一般是创建一个组
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="memberName"></param>
        /// <param name="optionAction"></param>
        /// <returns></returns>
        DtoToModelMapProfile<TModel, TDto, TPrimaryKey> CreateMember<TMember>(string memberName,
               Action<DtoToModelMapOption<TModel, TPrimaryKey>> optionAction);
        /// <summary>
        /// 隶属于模板类
        /// 一个Dto隶属于一个类,一个类可以有多个Dto
        /// </summary>
        /// <param name="tempClsType"></param>
        /// <returns></returns>
        DtoToModelMapProfile<TModel, TDto, TPrimaryKey> BelongTo(Type tempClsType);
        /// <summary>
        /// 设置TDTO的默认关键字过滤字段
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="selector">关键字过滤字段选择器</param>
        /// <returns></returns>
        DtoToModelMapProfile<TModel, TDto, TPrimaryKey> FilterKeywordWith(Expression<Func<TModel, object>> selector);
        /// <summary>
        /// 获取Dto和Model的映射器
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="destMember"></param>
        /// <returns></returns>
        DtoToModelMapValue<TModel, TPrimaryKey> GetMember<TMember>(Expression<Func<TDto, TMember>> destMember);
        /// <summary>
        /// 根据名称获取相应的映射对象
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="destMember"></param>
        /// <returns></returns>
        DtoToModelMapValue<TModel, TPrimaryKey> GetMember(string destMember);
    }
}

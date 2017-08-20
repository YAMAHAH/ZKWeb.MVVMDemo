using AutoMapper;
using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Components.GridSearchResponseBuilder;
using InfrastructurePlugins.BaseModule.Module;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.DtoToModelMap
{
    [ExportMany, SingletonReuse]
    public class DtoToModelMapper : IDtoToModelMapper
    {
        private ConcurrentDictionary<string, IDtoToModelMapProfileBase> xDtoToModelMaps = new ConcurrentDictionary<string, IDtoToModelMapProfileBase>();
        private ConcurrentDictionary<string, IMapper> xDtoMapperMaps = new ConcurrentDictionary<string, IMapper>();
        public IMapper GetDtoMapper<TDto>()
            where TDto : IOutputDto
        {
            var key = typeof(TDto).FullName;
            IMapper value;
            xDtoMapperMaps.TryGetValue(key, out value);
            return value == null ? null : value;
        }

        public bool AddOrUpdateMapper<TDto>(IMapper dtoMapper)
             where TDto : IOutputDto
        {
            if (dtoMapper == null) return false;
            var key = typeof(TDto).FullName;
            IMapper value = xDtoMapperMaps.AddOrUpdate(key, dtoMapper, (k, v) => v = dtoMapper);
            return value != null;
        }
        public IDtoToModelMapProfile<TModel, TDto, TPrimaryKey> GetDtoToModelMap<TModel, TDto, TPrimaryKey>()
            where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
        {
            var key = typeof(TDto).FullName;
            IDtoToModelMapProfileBase value;
            xDtoToModelMaps.TryGetValue(key, out value);
            //没有新增
            if (value == null)
            {
                value = new DtoToModelMapProfile<TModel, TDto, TPrimaryKey>();
                AddOrUpdateMap<TModel, TDto, TPrimaryKey>(value);
            }
            return value as IDtoToModelMapProfile<TModel, TDto, TPrimaryKey>;
        }
        public IDtoToModelMapProfileBase GetDtoToModelMap(Type dtoToModelMapType)
        {
            return (IDtoToModelMapProfileBase)ZKWeb.Application.Ioc.Resolve(dtoToModelMapType);
        }

        public bool AddOrUpdateMap<TModel, TDto, TPrimaryKey>(IDtoToModelMapProfileBase dtoToModelMap)
         where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
        {
            if (dtoToModelMap == null) return false;
            var key = dtoToModelMap.Name;
            IDtoToModelMapProfileBase value = xDtoToModelMaps.AddOrUpdate(key, dtoToModelMap, (k, v) => v = dtoToModelMap);
            return value != null;
        }
    }

    public class DtoToModelMapProfile<TModel, TDto, TPrimaryKey> : IDtoToModelMapProfile<TModel, TDto, TPrimaryKey>
        where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        private ConcurrentDictionary<string, DtoToModelMapValue<TModel, TPrimaryKey>> xDtoToModelMap = new ConcurrentDictionary<string, DtoToModelMapValue<TModel, TPrimaryKey>>();

        public string Name { get; set; }

        public Expression<Func<TModel, object>> KeywordFilterExpression { get; set; }

        public DtoToModelMapProfile()
        {
            this.Name = typeof(TDto).FullName;
            MakeTempClsObjectInfo();
        }

        private List<Type> typeLists = new List<Type>();
        /// <summary>
        /// 获取某个类型的所有属性(递归)
        /// </summary>
        /// <param name="tempClsType"></param>
        /// <returns></returns>
        protected List<EnumPropInfo> TraversalProperties(Type tempClsType)
        {
            var propinfos = new List<EnumPropInfo>();
            if (null == tempClsType) { return propinfos; }
            foreach (PropertyInfo pi in tempClsType.GetProperties(BindingFlags.Public |
                BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                PropertyInfo propInfo = tempClsType.GetProperty(pi.Name);

                var propType = propInfo.PropertyType;
                //if (propType.Equals(tempClsType)) return null;

                if (propType.IsArray || (propType.GetTypeInfo().IsClass &&
                    !propType.GetTypeInfo().IsGenericType &&
                    !propType.Equals(typeof(String)) &&
                    !propType.GetTypeInfo().IsValueType))
                {
                    if (!this.typeLists.Contains(propType))
                    {
                        this.typeLists.Add(propType);
                        propinfos.AddRange(TraversalProperties(propType));
                    }
                }
                else if (propType.GetTypeInfo().IsGenericType)
                {
                    var t = propType.GetGenericArguments()[0];
                    if (propType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    {
                        t = propType.GetGenericArguments()[1];
                    }
                    if (t.GetTypeInfo().IsClass &&
                        !t.GetTypeInfo().IsGenericType &&
                        !t.Equals(typeof(String)) &&
                        !t.GetTypeInfo().IsValueType)
                    {
                        if (!this.typeLists.Contains(t))
                        {
                            this.typeLists.Add(t);
                            propinfos.AddRange(TraversalProperties(t));
                        }
                    }
                    else
                    {
                        propinfos.Add(new EnumPropInfo() { ParentType = tempClsType, PropInfo = propInfo });
                    }
                }
                else
                {
                    propinfos.Add(new EnumPropInfo() { ParentType = tempClsType, PropInfo = propInfo });
                }
            }
            return propinfos;
        }
        private void MakeTempClsObjectInfo()
        {
            //根据模板类型自动生成相应的模板类对象信息
            var dtoType = typeof(TDto);
            //枚举实体的所有单层属性,生成相应的
            var objectInfos = TraversalProperties(dtoType);
            foreach (var objInfo in objectInfos)
            {
                var propInfo = objInfo.PropInfo;
                var propType = objInfo.PropInfo.PropertyType;
                var objAlias = objInfo.ParentType.Name.Replace("OutputDto", "") + "_" + propInfo.Name;
                var val = new DtoToModelMapValue<TModel, TPrimaryKey>()
                {
                    TemplateObjectInfo = new ComponentPropertyAttribute()
                    {
                        Name = propInfo.Name,
                        Alias = objAlias,
                        TempClassType = TemplateClassType,
                        DataType = propType.GetTypeInfo().IsGenericType ? propType.GetGenericArguments()[0].Name : propType.Name,
                        GroupType = objInfo.ParentType
                    },
                    ColumnType = propType,
                    Column = propInfo.Name
                };
                AddOrUpdate(objAlias, val);
            }
        }

        public void Register()
        {
            var mapper = ZKWeb.Application.Ioc.Resolve<IDtoToModelMapper>();
            mapper.AddOrUpdateMap<TModel, TDto, TPrimaryKey>(this);
        }

        public DtoToModelMapProfile<TModel, TDto, TPrimaryKey> ForMember<TMember>(Expression<Func<TDto, TMember>> destMember,
         Action<DtoToModelMapOption<TModel, TPrimaryKey>> optionAction)
        {
            var mapOptions = new DtoToModelMapOption<TModel, TPrimaryKey>();
            optionAction(mapOptions);
            var prop = destMember.Body.ToString().Split('.')[1];
            DtoToModelMapValue<TModel, TPrimaryKey> dtmMapValue;
            if (xDtoToModelMap.TryGetValue(prop, out dtmMapValue))
            {
                if (mapOptions.Expression != null) dtmMapValue.Expression = mapOptions.Expression;
                if (mapOptions.RefMapProfileType != null) dtmMapValue.RefMapProfileType = mapOptions.RefMapProfileType;
                if (mapOptions.ColumnFilter != null) dtmMapValue.ColumnFilter = mapOptions.ColumnFilter;
                if (mapOptions.ColumnFilterWrapper != null) dtmMapValue.ColumnFilterWrapper = mapOptions.ColumnFilterWrapper;
                if (mapOptions.ColumnFilterFunc != null) dtmMapValue.ColumnFilterFunc = mapOptions.ColumnFilterFunc;
                dtmMapValue.IsCustomColumnFilter = dtmMapValue.ColumnFilter != null || dtmMapValue.ColumnFilterWrapper != null;
                mapOptions.ObjectInfoAction?.Invoke(dtmMapValue.TemplateObjectInfo);
            }
            else
            {
                var value = new DtoToModelMapValue<TModel, TPrimaryKey>()
                {
                    Column = prop,
                    ColumnType = typeof(TMember),
                    Expression = mapOptions.Expression,
                    RefMapProfileType = mapOptions.RefMapProfileType,
                    ColumnFilter = mapOptions.ColumnFilter,
                    ColumnFilterWrapper = mapOptions.ColumnFilterWrapper,
                    IsCustomColumnFilter = mapOptions.ColumnFilter != null || mapOptions.ColumnFilterWrapper != null,
                    TemplateObjectInfo = mapOptions.TemplateClassObjectInfo,
                    ColumnFilterFunc = mapOptions.ColumnFilterFunc
                };
                AddOrUpdate(prop, value);
            }
            return this;
        }

        public DtoToModelMapProfile<TModel, TDto, TPrimaryKey> CreateMember<TMember>(string memberName,
                Action<DtoToModelMapOption<TModel, TPrimaryKey>> optionAction)
        {
            var mapOptions = new DtoToModelMapOption<TModel, TPrimaryKey>();
            optionAction(mapOptions);
            var prop = memberName;
            var value = new DtoToModelMapValue<TModel, TPrimaryKey>()
            {
                Column = prop,
                ColumnType = typeof(TMember),
                Expression = mapOptions.Expression,
                RefMapProfileType = mapOptions.RefMapProfileType,
                ColumnFilter = mapOptions.ColumnFilter,
                ColumnFilterWrapper = mapOptions.ColumnFilterWrapper,
                IsCustomColumnFilter = mapOptions.ColumnFilter != null || mapOptions.ColumnFilterWrapper != null,
                TemplateObjectInfo = mapOptions.TemplateClassObjectInfo,
                ColumnFilterFunc = mapOptions.ColumnFilterFunc
            };
            AddOrUpdate(prop, value);
            return this;
        }

        public DtoToModelMapValue<TModel, TPrimaryKey> CreateMapValue(string column, Type columnType, LambdaExpression expression)
        {
            return new DtoToModelMapValue<TModel, TPrimaryKey>()
            {
                Column = column,
                ColumnType = columnType,
                Expression = expression
            };
        }

        public void AddOrUpdate(string propName, DtoToModelMapValue<TModel, TPrimaryKey> value)
        {
            var result = xDtoToModelMap.AddOrUpdate(propName, value, (k, v) => v = value);
        }

        public DtoToModelMapProfile<TModel, TDto, TPrimaryKey> FilterKeywordWith(Expression<Func<TModel, object>> selector)
        {
            KeywordFilterExpression = selector;
            return this;
        }

        public DtoToModelMapValue<TModel, TPrimaryKey> GetMember<TMember>(Expression<Func<TDto, TMember>> destMember)
        {
            var propKey = destMember.Body.ToString().Split('.')[1];
            DtoToModelMapValue<TModel, TPrimaryKey> expValue;
            xDtoToModelMap.TryGetValue(propKey, out expValue);
            return expValue;
        }

        public DtoToModelMapValue<TModel, TPrimaryKey> GetMember(string destMember)
        {
            var propKey = destMember;
            DtoToModelMapValue<TModel, TPrimaryKey> expValue;
            xDtoToModelMap.TryGetValue(propKey, out expValue);
            return expValue;
        }

        public Type TemplateClassType { get; set; }
        public DtoToModelMapProfile<TModel, TDto, TPrimaryKey> BelongTo(Type tempClsType)
        {
            TemplateClassType = tempClsType;
            return this;
        }
    }

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

    public interface IDtoToModelMapProfileBase
    {
        string Name { get; set; }
        void Register();
    }

    public class DtoToModelMapValue<TModel, TPrimaryKey>
         where TModel : class, IEntity, IEntity<TPrimaryKey>
    {
        public string Column { get; set; }

        public Type ColumnType { get; set; }

        public LambdaExpression Expression { get; set; }
        public Type RefMapProfileType { get; set; }

        public QueryColumnFilterDelegate<TModel, TPrimaryKey> ColumnFilter { get; set; }

        public ColumnFilterWrapperDelegate<TModel, TPrimaryKey> ColumnFilterWrapper { get; set; }

        public bool IsCustomColumnFilter { get; set; }
        public QueryColumnFilterFunc<TModel, TPrimaryKey> ColumnFilterFunc { get; set; }

        public ComponentPropertyAttribute TemplateObjectInfo { get; set; }
    }

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
        /// 引用配置文件类型
        /// </summary>
        public Type RefMapProfileType { get; set; }

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
        /// 引用映射配置文件
        /// </summary>
        /// <param name="mapProfileType"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public DtoToModelMapOption<TModel, TPrimaryKey> RefMapProfile(Type mapProfileType, string parent = null)
        {
            RefMapProfileType = mapProfileType;
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
    public interface IDtoToModelMapper
    {
        IDtoToModelMapProfile<TModel, TDto, TPrimaryKey> GetDtoToModelMap<TModel, TDto, TPrimaryKey>()
           where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>;
        bool AddOrUpdateMap<TModel, TDto, TPrimaryKey>(IDtoToModelMapProfileBase dtoToModelMap)
          where TDto : IOutputDto where TModel : class, IEntity, IEntity<TPrimaryKey>;

        IMapper GetDtoMapper<TDto>() where TDto : IOutputDto;
        bool AddOrUpdateMapper<TDto>(IMapper dtoMapper) where TDto : IOutputDto;
    }
    public interface IValueResolver<in TSource, in TDestination, TDestMember>
    {
        TDestMember Resolve(TSource source, TDestination destination, TDestMember destMember);
    }
}

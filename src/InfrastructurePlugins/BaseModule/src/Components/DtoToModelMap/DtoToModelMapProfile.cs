using InfrastructurePlugins.BaseModule.Application.Attributes;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Components.QueryBuilder;
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
        protected List<EnumPropInfo> TraversalProperties(Type tempClsType, EnumPropInfo parentInfo)
        {
            var propinfos = new List<EnumPropInfo>();
            if (null == tempClsType) { return propinfos; }
            var modelType = tempClsType.GetTypeInfo().GetCustomAttribute<ModelTypeMapperAttribute>()?.ModelType ?? tempClsType;

            foreach (PropertyInfo pi in tempClsType.GetProperties(BindingFlags.Public |
                BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                PropertyInfo propInfo = pi;
                var propType = pi.PropertyType;
                var prefix = parentInfo == null ? string.Empty : parentInfo.Prefix == string.Empty ? parentInfo.PropInfo.Name : string.Join(".", parentInfo.Prefix, parentInfo.PropInfo.Name);

                if (propType.IsArray || (propType.GetTypeInfo().IsClass &&
                    !propType.GetTypeInfo().IsGenericType &&
                    !propType.Equals(typeof(String)) &&
                    !propType.GetTypeInfo().IsValueType))
                {
                    if (!this.typeLists.Contains(propType))
                    {
                        this.typeLists.Add(propType);
                        var enumPropInfo = new EnumPropInfo()
                        {
                            Prefix = prefix,
                            ParentModelType = parentInfo?.ModelType,
                            ModelType = modelType,
                            DtoEntityType = tempClsType,
                            ParentPropInfo = parentInfo?.PropInfo,
                            PropInfo = propInfo,
                            PropClassify = propType.IsArray ? PropClassify.List : PropClassify.Object
                        };
                        propinfos.Add(enumPropInfo);
                        propinfos.AddRange(TraversalProperties(propType, enumPropInfo));
                    }
                }
                else if (propType.GetTypeInfo().IsGenericType)
                {
                    var genericType = propType.GetGenericArguments()[0];
                    if (propType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    {
                        genericType = propType.GetGenericArguments()[1];
                    }
                    if (genericType.GetTypeInfo().IsClass &&
                        !genericType.GetTypeInfo().IsGenericType &&
                        !genericType.Equals(typeof(String)) &&
                        !genericType.GetTypeInfo().IsValueType)
                    {
                        if (!this.typeLists.Contains(genericType))
                        {
                            this.typeLists.Add(genericType);
                            var enumPropInfo = new EnumPropInfo()
                            {
                                Prefix = prefix,
                                ParentModelType = parentInfo?.ModelType,
                                ModelType = modelType,
                                DtoEntityType = tempClsType,
                                ParentPropInfo = parentInfo?.PropInfo,
                                PropInfo = propInfo,
                                PropClassify = PropClassify.List
                            };
                            propinfos.Add(enumPropInfo);
                            propinfos.AddRange(TraversalProperties(genericType, enumPropInfo));
                        }
                    }
                    else
                    {
                        propinfos.Add(new EnumPropInfo()
                        {
                            Prefix = prefix,
                            ParentModelType = parentInfo?.ModelType,
                            ModelType = modelType,
                            DtoEntityType = tempClsType,
                            ParentPropInfo = parentInfo?.PropInfo,
                            PropInfo = propInfo,
                            PropClassify = PropClassify.List
                        });
                    }
                }
                else
                {
                    propinfos.Add(new EnumPropInfo()
                    {
                        Prefix = prefix,
                        ParentModelType = parentInfo?.ModelType,
                        ModelType = modelType,
                        DtoEntityType = tempClsType,
                        ParentPropInfo = parentInfo?.PropInfo,
                        PropInfo = propInfo,
                        PropClassify = PropClassify.Basic
                    });
                }
            }
            return propinfos;
        }
        Dictionary<Type, ILambdaExpressionBuilderBase> typeBuilders = new Dictionary<Type, ILambdaExpressionBuilderBase>();
        private void MakeTempClsObjectInfo()
        {
            //根据模板类型自动生成相应的模板类对象信息
            var dtoType = typeof(TDto);
            //枚举实体的所有单层属性,生成相应的
            var objectInfos = TraversalProperties(dtoType, null);

            foreach (var objInfo in objectInfos)
            {
                var propInfo = objInfo.PropInfo;
                var propType = objInfo.PropInfo.PropertyType;
                var objAlias = objInfo.DtoEntityType.Name.Replace("OutputDto", "") + "_" + propInfo.Name;
                var val = new DtoToModelMapValue<TModel, TPrimaryKey>()
                {
                    TemplateObjectInfo = new ComponentPropertyAttribute()
                    {
                        Name = propInfo.Name,
                        Alias = objAlias,
                        TempClassType = TemplateClassType,
                        DataType = propType.GetTypeInfo().IsGenericType ? propType.GetGenericArguments()[0].Name : propType.Name,
                        GroupType = objInfo.DtoEntityType,
                        PropertyType = objInfo.PropClassify
                    },
                    ColumnType = propType,
                    ColumnName = propInfo.Name,
                    DtoEntityType = objInfo.DtoEntityType,
                    PropertyClassify = objInfo.PropClassify,
                    ExpressionBuilder = CreateLambdaExpressionBuilder(objInfo.ModelType ?? objInfo.DtoEntityType),
                    ParentExpressionBuilder = objInfo.ParentModelType == null ? null : CreateLambdaExpressionBuilder(objInfo.ParentModelType),
                    ModelType = objInfo.ModelType,
                    ParentModelType = objInfo.ParentModelType,
                    Prefix = objInfo.Prefix
                };
                AddOrUpdate(objAlias, val);
            }
        }
        /// <summary>
        /// 创建表达式生成器
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        private ILambdaExpressionBuilderBase CreateLambdaExpressionBuilder(Type entityType)
        {
            //按约定获取指定的实体类型
            if (typeBuilders.ContainsKey(entityType)) return typeBuilders[entityType];

            var builderType = typeof(LambdaExpressionBuilder<>).MakeGenericType(entityType);
            var builder = (ILambdaExpressionBuilderBase)Activator.CreateInstance(builderType);
            typeBuilders[entityType] = builder;

            return builder;
        }
        public IContainer Injector { get; } = ZKWeb.Application.Ioc;

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
                    ColumnName = prop,
                    ColumnType = typeof(TMember),
                    Expression = mapOptions.Expression,
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
                ColumnName = prop,
                ColumnType = typeof(TMember),
                Expression = mapOptions.Expression,
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
                ColumnName = column,
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
}

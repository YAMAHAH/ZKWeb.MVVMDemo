using AutoMapper;
using InfrastructurePlugins.BaseModule.Application.Dtos;
using InfrastructurePlugins.BaseModule.Application.Mappers;
using InfrastructurePlugins.BaseModule.Components.DtoToModelMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    /// <summary>
    /// 查询表达式创建工厂
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class ExpressionCreateFactory<TEntity, TDto, TPrimaryKey>
         where TEntity : class, IEntity, IEntity<TPrimaryKey> where TDto : IOutputDto
    {
        protected IContainer Injector => ZKWeb.Application.Ioc;

        protected IMapper xMapper;
        public IMapper Mapper { get { return xMapper; } }
        public ExpressionCreateFactory()
        {
            xMapper = new ColumnFilterMapperFactory<TEntity, TDto, TPrimaryKey>().CreateMapper();
        }
        /// <summary>
        /// 解析条件生成对应的结点树
        /// 前端对象->转换后端条件对象->生成集合结点->解析对象生成相应的结点树
        /// 要考虑非自定义的条件
        /// 集合查询不支持子表达式??
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private List<ColumnQueryCondition> ParseFilterAndCreateTree(ColumnQueryCondition[] filters)
        {
            Dictionary<string, ColumnQueryCondition> filterTreeMaps = new Dictionary<string, ColumnQueryCondition>();
            List<string> rootNodes = new List<string>();
            foreach (var filter in filters)
            {
                var newFilter = filter;
                //如果是自定义直接返回
                if (newFilter.IsCustomColumnFilter)
                {
                    //PropertyName应该是全称,包含前缀
                    rootNodes.Add(newFilter.MemberName);
                    filterTreeMaps[newFilter.MemberName] = newFilter;
                    continue;
                }
                var dtmMapper = Injector.Resolve<IDtoToModelMapper>();
                var dtmMapProfile = dtmMapper.GetDtoToModelMap<TEntity, TDto, TPrimaryKey>();

                var splitNames = newFilter.Prefix.Split('.');
                var nodeNames = new List<string>();
                var j = splitNames.Length;
                while (j > -1)
                {
                    var joinStr = splitNames[0];
                    for (int i = 1; i < j; i++)
                    {
                        joinStr = string.Join(".", joinStr, splitNames[i]);
                    }
                    nodeNames.Add(joinStr);
                    j--;
                }

                var rootNode = nodeNames.FirstOrDefault();
                if (!rootNodes.Contains(rootNode)) rootNodes.Add(rootNode);

                var preNode = string.Empty;
                bool isCustomeFilter = false;

                foreach (var nodeName in nodeNames)
                {
                    //获取成员全称
                    var mapValue = dtmMapProfile.GetMember(nodeName);
                    var newNode = new ColumnQueryCondition()
                    {
                        PropertyName = nodeName,
                        ProperyType = mapValue.ColumnType,
                        Prefix = mapValue.Prefix,
                        ModelType = mapValue.ModelType,
                        ParentModelType = mapValue.ParentModelType,
                        ExpressionBuilder = mapValue.ExpressionBuilder,
                        ParentExpressionBuilder = mapValue.ParentExpressionBuilder,
                        PropClassify = mapValue.PropertyClassify,
                        SetOpertion = newFilter.SetOpertion,
                        SrcExpression = mapValue.Expression,
                        IsCustomColumnFilter = mapValue.IsCustomColumnFilter,
                        IsSetNode = mapValue.IsSetNode,
                        IsSetOperation = mapValue.IsSetNode && mapValue.PropertyClassify == Module.PropClassify.List
                    };
                    filterTreeMaps[nodeName] = newNode;
                    if (!string.IsNullOrEmpty(preNode))
                    {
                        filterTreeMaps[preNode].Childs.Add(newNode);
                    }
                    preNode = nodeName;
                    if (isCustomeFilter) break; //若结点是自定义条件,则直接退出
                }

                if (!string.IsNullOrEmpty(preNode) && !isCustomeFilter) filterTreeMaps[preNode].Childs.Add(newFilter);
            }
            //获取根的所有值
            var results = filterTreeMaps.Where(kv => rootNodes.Any(r => r == kv.Key)).Select(kv => kv.Value).ToList();

            return results;
        }
        /// <summary>
        /// 创建表达式
        /// </summary>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> CreateQueryExpression(params GridSearchColumnFilter[] columnFilters)
        {
            var root = new ColumnQueryCondition() { IsChildExpress = true };
            var colFilters = columnFilters.ToList();
            //前端转换为后端的对象
            var cqconds = xMapper.Map<List<ColumnQueryCondition>>(colFilters);
            //处理集合结点对象,分析出有多个结点
            var setNodes = cqconds.Where(c => c.IsSetNode).ToList();
            foreach (var item in setNodes)
            {
                var newNode = ParseFilterAndCreateTree(item);

            }
            //用新结点去替换原来的结点(item)
            //获取原来结点的位置
            //删除原来的结点
            //

            root.Childs.AddRange(cqconds);
            var expBuilder = new LambdaExpressionBuilder<TEntity>();
            var rootExpr = expBuilder.GenerateLambdaExpression(root);
            return rootExpr;
        }

        /// <summary>
        /// 创建Lambda表达式
        /// </summary>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public Func<TEntity, bool> CreateQueryLambda(params GridSearchColumnFilter[] columnFilters)
        {
            return CreateQueryExpression(columnFilters).Compile();
        }

        /// <summary>
        /// 创建子查询表达式
        /// </summary>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> CreateChildQueryExpression(params GridSearchColumnFilter[] columnFilters)
        {
            var root = new ColumnQueryCondition() { IsChildExpress = true };
            var colFilters = columnFilters.Where(f => f.IsChildQuery).ToList();
            var cqconds = xMapper.Map<List<ColumnQueryCondition>>(colFilters);

            root.Childs.AddRange(cqconds);
            var expBuilder = new LambdaExpressionBuilder<TEntity>();
            var rootExpr = expBuilder.GenerateLambdaExpression(root);
            return rootExpr;
        }

        /// <summary>
        /// 创建子查询Lambda表达式
        /// </summary>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public Func<TEntity, bool> CreateChildQueryLambda(params GridSearchColumnFilter[] columnFilters)
        {
            return CreateChildQueryExpression(columnFilters).Compile();
        }

        public LambdaExpressionBuilder<TEntity> CreateBuilder()
        {
            return new LambdaExpressionBuilder<TEntity>();
        }
        public static object CreateBuilder(Type type)
        {
            var builderType = typeof(LambdaExpressionBuilder<>).MakeGenericType(type);
            return Activator.CreateInstance(builderType);
        }
        public static object MakeExpressionCreateFactory(Type entityType, Type dtoType, Type primaryType)
        {
            var genericType = typeof(LambdaExpressionBuilder<>).MakeGenericType(entityType, dtoType, primaryType);
            return Activator.CreateInstance(genericType);
        }
        public static dynamic TryParserArray(string[] values, Type t)
        {
            //string[] searchArray = value.Split(',');
            var genericType = typeof(List<>).MakeGenericType(t);
            var list = Activator.CreateInstance(genericType);
            var addMethod = genericType.GetMethod("Add");

            foreach (var l in values)
            {
                try
                {
                    var dValue = Convert.ChangeType(l, t);
                    //dList.Add(dValue);
                    addMethod.Invoke(list, new object[] { dValue });
                }
                catch { }
            }
            return list;
        }
    }
}

using InfrastructurePlugins.BaseModule.Module;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfrastructurePlugins.BaseModule.Components.QueryBuilder
{
    public class ColumnQueryCondition
    {
        /// <summary>
        /// 自定义列过滤条件
        /// </summary>
        public bool IsCustomColumnFilter { get; set; }
        /// <summary>
        /// 子表达式
        /// </summary>
        public bool IsChildExpress { get; set; }

        /// <summary>
        /// 子查询
        /// </summary>
        public bool IsChildQuery { get; set; }
        /// <summary>
        /// 指示是否聚合函数
        /// </summary>
        public bool IsSetOperation { get; set; }
        /// <summary>
        /// 指示是否是集合结点
        /// </summary>

        public bool IsSetNode { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public Type ProperyType { get; set; } = typeof(object);
        /// <summary>
        /// 父域模型类型
        /// </summary>
        public Type ParentModelType { get; set; }
        /// <summary>
        /// 域模型类型
        /// </summary>
        public Type ModelType { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 属性分类
        /// </summary>
        public PropClassify PropClassify { get; set; }
        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 成员名称,前缀和列名的组合
        /// </summary>
        public string MemberName
        {
            get
            {
                return string.IsNullOrEmpty(Prefix) ? PropertyName : (Prefix + "." + PropertyName).Trim();
            }
        }
        /// <summary>
        /// 表达式生成器
        /// </summary>
        public ILambdaExpressionBuilderBase ExpressionBuilder { get; set; }
        /// <summary>
        /// 父结点表达式生成器
        /// </summary>
        public ILambdaExpressionBuilderBase ParentExpressionBuilder { get; set; }
        /// <summary>
        /// 指示是否处理完成
        /// </summary>
        public bool IsProcessDone { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public OpertionSymbol OpertionSymbol { get; set; }

        /// <summary>
        /// 集合操作符
        /// </summary>
        public SetOpertionSymbol SetOpertion { get; set; }
        /// <summary>
        /// 值1
        /// </summary>
        public object Value1 { get; set; }
        /// <summary>
        /// 值2
        /// </summary>
        public object Value2 { get; set; }
        /// <summary>
        /// 逻辑连接符
        /// </summary>
        public ConcatType Concat { get; set; }
        /// <summary>
        /// 子表达式查询条件
        /// </summary>
        public List<ColumnQueryCondition> Childs { get; set; } = new List<ColumnQueryCondition>();
        /// <summary>
        /// 条件表达式树
        /// </summary>
        public Expression Expression { get; set; }

        /// <summary>
        /// 源条件表达式树
        /// </summary>
        public LambdaExpression SrcExpression { get; set; }
        /// <summary>
        /// 正则表达式
        /// </summary>
        public string RegExp { get; set; }
    }
}

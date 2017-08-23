using InfrastructurePlugins.BaseModule.Module;
using System;

namespace InfrastructurePlugins.BaseModule.Application.Attributes
{
    /// <summary>
    /// 组件属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ComponentPropertyAttribute : Attribute
    {
        /// <summary>
        /// 源实体路径
        /// a.b表示a对象的b属性
        /// </summary>
        public string SrcModelPath { get; set; }
        /// <summary>
        /// 源实体的属性
        /// </summary>
        public string SrcModelProp { get; set; }
        /// <summary>
        /// 组别类型
        /// </summary>
        public Type GroupType { get; set; }
        /// <summary>
        /// 根据名称和属性计算
        /// </summary>
        public Guid ParentId { get; set; }
        public string Parent { get; set; }
        /// <summary>
        /// 模板类类型
        /// </summary>
        public Type TempClassType { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public PropClassify PropertyType { get; set; }
        public string TempName { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public bool Queryable { get; set; } = true;
        public bool required { get; set; } = false;
        public bool Visible { get; set; } = true;
        public bool Editable { get; set; } = true;
        public string Default { get; set; }
        public string DataType { get; set; }
        public string ComponentType { get; set; }

        public ComponentPropertyAttribute()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructurePlugins.BaseModule.Domain.Services
{
    public interface ITreeConfig<TEntity, TPrimaryKey>
    {
        /// <summary>
        /// 创建新的结点
        /// </summary>
        Func<TEntity, TEntity> GetNewNode { get; set; }
        //bs1 数据库存在 BS2 现有的结点
        /// <summary>
        /// 保留存在结点的值
        /// </summary>
        Action<TEntity, TEntity> GetConfig { get; set; }
        /// <summary>
        /// 按主键值来查找实体
        /// </summary>
        Func<TEntity, TEntity, bool> GetKey { get; set; }
        /// <summary>
        /// 获取结点的孩子
        /// </summary>
        Func<TEntity, List<TEntity>> GetChilds { get; set; }
        /// <summary>
        /// 用于比较的键
        /// </summary>
        Func<TEntity, TPrimaryKey> GetCompareKey { get; set; }
    }
}

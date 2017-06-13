using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services
{
    public class TreeNodeConfig<TEntity, TPrimaryKey> : ITreeConfig<TEntity, TPrimaryKey> where TEntity : IHaveTreeNode<TPrimaryKey>,
        IHaveChildren<TPrimaryKey>, IEntity<TPrimaryKey>, new()
    {
        /// <summary>
        /// 创建新的结点
        /// </summary>
        Func<TEntity, TEntity> ITreeConfig<TEntity, TPrimaryKey>.GetNewNode { get; set; } = pNode => new TEntity()
        {
            Parent = pNode,
            ParentId = pNode.Id,
            RootId = pNode.RootId
        };
        //bs1 数据库存在 BS2 现有的结点
        /// <summary>
        /// 保留存在结点的值
        /// existNode：存在的结点 nowNode: 当前的结点
        /// </summary>
        Action<TEntity, TEntity> ITreeConfig<TEntity, TPrimaryKey>.GetConfig { get; set; } = (existNode, nowNode) =>
        {
            nowNode.Id = existNode.Id;
            nowNode.RootId = existNode.RootId;
        };
        /// <summary>
        /// 按主键值来查找实体
        /// </summary>
        Func<TEntity, TEntity, bool> ITreeConfig<TEntity, TPrimaryKey>.GetKey { get; set; } = (bNode1, bNode2) => bNode1.Id.Equals(bNode2.Id);
        /// <summary>
        /// 获取结点的孩子
        /// </summary>
        Func<TEntity, List<TEntity>> ITreeConfig<TEntity, TPrimaryKey>.GetChilds { get; set; } = bNode => bNode.Childs as List<TEntity>;
        /// <summary>
        /// 用于比较的键
        /// </summary>
        Func<TEntity, TPrimaryKey> ITreeConfig<TEntity, TPrimaryKey>.GetCompareKey { get; set; } = bNode => bNode.Id;
    }
}

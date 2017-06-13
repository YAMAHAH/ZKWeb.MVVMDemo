using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services
{
    public class TreeNodeVersionConfig<TEntity, TPrimaryKey> : ITreeConfig<TEntity, TPrimaryKey> where TEntity : IHaveTreeNode<TPrimaryKey>,
        IHaveChildren<TPrimaryKey>, IHaveRootVersion<TPrimaryKey>, IHaveNodeVersion<TPrimaryKey>, IEntity<TPrimaryKey>, new()
    {
        /// <summary>
        /// 创建新的结点
        /// </summary>
        public Func<TEntity, TEntity> GetNewNode { get; set; } = pNode => new TEntity()
        {
            Parent = pNode,
            ParentId = pNode.Id,
            RootVersionId = pNode.RootVersionId,
            RootId = pNode.RootId
        };
        //bs1 数据库存在 BS2 现有的结点
        /// <summary>
        /// 保留存在结点的值
        /// </summary>
        public Action<TEntity, TEntity> GetConfig { get; set; } = (existNode, nowNode) =>
        {
            nowNode.Id = existNode.Id;
            nowNode.RootVersionId = existNode.RootVersionId;
            nowNode.RootId = existNode.RootId;
        };
        /// <summary>
        /// 按主键值来查找实体
        /// </summary>
        public Func<TEntity, TEntity, bool> GetKey { get; set; } = (bNode1, bNode2) => bNode1.Id.Equals(bNode2.Id);
        /// <summary>
        /// 获取结点的孩子
        /// </summary>
        public Func<TEntity, List<TEntity>> GetChilds { get; set; } = bNode => bNode.Childs as List<TEntity>;
        /// <summary>
        /// 用于比较的键
        /// </summary>
        public Func<TEntity, TPrimaryKey> GetCompareKey { get; set; } = bNode => bNode.Id;
    }
}

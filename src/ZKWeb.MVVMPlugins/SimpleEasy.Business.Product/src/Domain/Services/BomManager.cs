using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Bases;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow;
using ZKWeb.MVVMPlugins.SimpleEasy.Business.Product.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.SimpleEasy.Business.Product.src.Domain.Services
{
    /// <summary>
    /// BOM管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class BomManager : DomainServiceBase<Bom, Guid>
    {
        /// <summary>
        /// 根据产品版次查找所有引用的结点
        /// </summary>
        /// <param name="productVserionId">版次ID</param>
        private List<Bom> GetAllRefNodeByVersion(string productVserionId)
        {
            return UnitRepository.RawQuery("CALL getNodesByVersion({0})", productVserionId).ToList();
        }
        /// <summary>
        /// 获取指定版次和根的结点
        /// </summary>
        /// <param name="productVserionId">版次ID</param>
        /// <param name="rootId">根ID</param>
        /// <returns></returns>
        private List<Bom> GetBomNodesByVersion(string productVserionId, string rootId)
        {
            return UnitRepository.RawQuery("select * from bom where RootProductVersionId = {0} and RootId = {1}", productVserionId, rootId).ToList();
        }

        /// <summary>
        /// 更新所有引用指定版次的结点值
        /// </summary>
        /// <param name="nowRootId"></param>
        /// <param name="nowVersionId"></param>
        /// <param name="oldVersionId"></param>
        private void UpdateAllRefNodeByVersion(string nowRootId, string nowVersionId, string oldVersionId)
        {
            var nowNodes = GetBomNodesByVersion(nowVersionId, nowRootId);
            var allRefNodes = GetAllRefNodeByVersion(oldVersionId);
            var nowRootNode = nowNodes.Find(nd => nd.ParentId == null && nd.NodeVersionId == nd.RootVersionId);

            Func<Bom, Bom> getNewNode = bNode => new Bom()
            {
                Parent = bNode,
                ParentId = bNode.Id,
                RootVersionId = bNode.RootVersionId,
                RootId = bNode.RootId
            };
            Action<Bom, Bom> getConfig = (bNode1, bNode2) =>
            {
                bNode2.Id = bNode1.Id;
                bNode2.RootVersionId = bNode1.RootVersionId;
                bNode2.RootId = bNode1.RootId;
            };
            Func<Bom, Bom, bool> getKey = (bNode1, bNode2) => bNode1.Id == bNode2.Id;
            Func<Bom, List<Bom>> getChilds = bNode => bNode.Childs;
            Func<Bom, Guid> getCompareKey = bNode => bNode.Id;

            foreach (var nd in allRefNodes.Where(nd => nd.NodeVersionId.Equals(Guid.Parse(oldVersionId))))
            {
                UnitRepository.UpdateTreeNode(nd, nowRootNode, getChilds, getCompareKey, getKey, getNewNode, getConfig);
                // updateNodeOrder(nd.RootId, nd.RootId);
                var allNodes = UnitRepository.RawQuery("CALL getBomNodes({0},{1})", nd.RootId.ToString(), nd.RootId).ToList();

                var rootNode = allNodes.Find(bnd => bnd.Id == nd.RootId);
                if (rootNode != null)
                {
                    CalaOrder(rootNode);
                }
            }
            UnitOfWork.SaveChanges();
        }
        /// <summary>
        /// 更新结点的基本信息
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="rootId"></param>
        private void updateNodeOrder(Guid nodeId, Guid rootId)
        {
            var allNodes = UnitRepository.RawQuery("CALL getBomNodes({0},{1})", nodeId, rootId).ToList();

            var rootNode = allNodes.Find(nd => nd.Id == nodeId);
            if (rootNode != null)
            {
                CalaOrder(rootNode);
                //保存数据
                UnitOfWork.SaveChanges();
            }
        }

        /// <summary>
        /// 计算序号,左序,右序,层次,用量
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        /// 
        protected override NodeOrderInfo CalaOrder(Bom treeNode)
        {
            treeNode.Level = 1;
            treeNode.Total = 1;
            Action<Bom, NodeOrderInfo> headerConfig = (rpt, nodeInfo) =>
            {
                rpt.Ord = nodeInfo.Ord;
                rpt.Lft = nodeInfo.InitNum;
            };
            Action<Bom, Bom> middleConfig = (curNode, parentNode) =>
            {
                curNode.Level = parentNode.Level + 1;
                curNode.Total = parentNode.Total * curNode.SingleTotal;
            };
            Action<Bom, int> footerConfig = (nd, initNum) =>
            {
                nd.Rgt = initNum;
            };
            Func<Bom, IEnumerable<Bom>> getChilds = (nd) => nd.Childs;
            return UnitRepository.CalaOrd(treeNode, headerConfig, middleConfig, footerConfig, getChilds, new NodeOrderInfo());
        }

        /// <summary>
        /// 创建或更新根结点
        /// </summary>
        /// <param name="rootNode">更新结点</param>
        /// <returns></returns>
        public Bom UpsertRootBomNode(Bom rootNode)
        {
            Func<Bom, Bom> getNewNode = bs => new Bom()
            {
                Parent = bs,
                ParentId = bs.Id,
                RootVersionId = bs.RootVersionId,
                RootId = bs.RootId
            };
            //bs1 数据库存在 BS2 现有的结点
            Action<Bom, Bom> getConfig = (bs1, bs2) =>
            {
                bs2.Id = bs1.Id;
                bs2.RootVersionId = bs1.RootVersionId;
                bs2.RootId = bs1.RootId;
            };
            Func<Bom, Bom, bool> getKey = (bNode1, bNode2) => bNode1.Id == bNode2.Id;
            Func<Bom, List<Bom>> getChilds = bNode => bNode.Childs;
            Func<Bom, Guid> getCompareKey = bNode => bNode.Id;

            var nowBomNode = rootNode;

            if (nowBomNode.Id == Guid.Empty || nowBomNode.Id.Equals(DBNull.Value))
            {
                var existBomNode = new Bom() { Parent = null, ParentId = null, RootVersionId = nowBomNode.NodeVersionId };
                UnitRepository.Upsert(ref existBomNode);
                existBomNode.RootId = existBomNode.Id;
                UnitRepository.AddTreeNode(existBomNode, nowBomNode, getChilds, getNewNode, getConfig);
                nowBomNode = existBomNode;
            }
            else
            {
                var existBomNodeList = UnitRepository.Query()
                        .Where(bomNd => bomNd.RootId == nowBomNode.Id)
                        .ToList();

                var existBomNode = existBomNodeList.FirstOrDefault(bomNd => bomNd.Id == nowBomNode.Id && bomNd.ParentId == null);
                nowBomNode.Parent = null;
                nowBomNode.ParentId = null;
                UnitRepository.UpdateTreeNode(existBomNode, nowBomNode, getChilds, getCompareKey, getKey, getNewNode, getConfig);
                nowBomNode = existBomNode;
            }
            CalaOrder(nowBomNode); //计算序号，层次，结点左序号，结点右序号,数量
                                   // CalaNeedTotal(nowBomNode); //计算数量
            UnitOfWork.SaveChanges();
            if (nowBomNode.RootId == Guid.Empty || nowBomNode.RootId.Equals(DBNull.Value))
            {
                // var retCount = UnitOfWork.ExecuteSqlCommand("update Reports set RootId = {1} where RootId ={0}", nowReport.RootId, nowReport.ReportId);
                UnitRepository.UpdateLocalNodeValues(nowBomNode, (nd) => nd.Childs, (nd) => nd.RootId = nowBomNode.Id);
                UnitOfWork.SaveChanges();
            }
            return nowBomNode;
        }

        /// <summary>
        /// 创建或更新子结点
        /// </summary>
        /// <param name="childNode">子结点</param>
        /// <returns></returns>
        public Bom UpsertBomNode(Bom childNode)
        {
            if (childNode.ParentId == null || childNode.Id == Guid.Empty)
            {
                throw new NullReferenceException("BOM结点的父结点外键或结点的主键为空");
            }
            //创建新结点
            Func<Bom, Bom> getNewNode = bNode => new Bom()
            {
                Parent = bNode,
                ParentId = bNode.Id,
                RootVersionId = bNode.RootVersionId,
                RootId = bNode.RootId
            };
            //配置结点的值,bNode1数据库存在的结点，bNode2现在的结点
            Action<Bom, Bom> getConfig = (bNode1, bNode2) =>
            {
                bNode2.Id = bNode1.Id;
                bNode2.RootVersionId = bNode1.RootVersionId;
                bNode2.RootId = bNode1.RootId;
            };
            //查找结点的函数
            Func<Bom, Bom, bool> getKey = (bNode1, bNode2) => bNode1.Id == bNode2.Id;
            //结点的子结点集合
            Func<Bom, List<Bom>> getChilds = bNode => bNode.Childs;
            //结点比较函数
            Func<Bom, Guid> getCompareKey = bNode => bNode.Id;

            var nowBomNode = childNode;

            if (nowBomNode.Id.Equals(DBNull.Value) || nowBomNode.Id == Guid.Empty) //<= 0 
            {
                var existBNode = new Bom()
                {
                    Parent = nowBomNode.Parent,
                    ParentId = nowBomNode.Parent.ParentId,
                    RootVersionId = nowBomNode.RootVersionId,
                    RootId = nowBomNode.RootId
                };
                //插入结点
                UnitRepository.Upsert(ref existBNode);
                //递归设置子结点的值
                UnitRepository.AddTreeNode(existBNode, nowBomNode, getChilds, getNewNode, getConfig);
                nowBomNode = existBNode;
            }
            else
            {
                var existBomNodeList = UnitRepository.RawQuery("CALL getBomNodes({0},{1})", childNode.Id, childNode.RootId).ToList();

                var existBNode = existBomNodeList.FirstOrDefault(r => r.Id == childNode.Id);
                //递归更新结点值
                UnitRepository.UpdateTreeNode(existBNode, nowBomNode, getChilds, getCompareKey, getKey, getNewNode, getConfig);
                nowBomNode = existBNode;
            }
            //保存结点后更新结点序号,开启事务后,可以不用,有待测试
            UnitOfWork.SaveChanges();
            //更新结点序号
            updateNodeOrder(nowBomNode.RootId, nowBomNode.RootId);
            return nowBomNode;
        }
        /// <summary>
        /// 删除结点
        /// </summary>
        /// <param name="delBom"></param>
        public void DeleteBomNode(ref Bom delBom)
        {
            var nowNode = delBom;

            var existNodeLists = UnitRepository?.RawQuery("CALL getBomNodes({0},{1})", delBom.Id, delBom.RootId).ToList();

            var existNode = existNodeLists.Where(r => r.Id == nowNode.Id).FirstOrDefault();

            UnitRepository.DeleteTreeNode(existNode, (nd) => nd.Childs);

            UnitOfWork.SaveChanges();

            if (delBom.Id != delBom.RootId)
            {
                //找出根结点和根版次 
                updateNodeOrder(delBom.RootId, delBom.RootId);

                UnitOfWork.SaveChanges();
            }
        }
    }
}

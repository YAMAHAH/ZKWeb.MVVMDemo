using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Components.Exceptions;
using InfrastructurePlugins.BaseModule.Domain.Services.Bases;
using InfrastructurePlugins.BaseModule.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Ioc;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 报表管理
    /// </summary>
    [ExportMany, SingletonReuse]
    public class ReportManager : DomainServiceBase<Report, Guid>, IReportManager
    {
        protected override NodeOrderInfo CalaOrder(Report treeNode)
        {
            treeNode.Level = 1;
            Action<Report, NodeOrderInfo> headerConfig = (rpt, nodeInfo) =>
            {
                rpt.Ord = nodeInfo.Ord;
                rpt.Lft = nodeInfo.InitNum;
                //if (rpt.Parent == null) rpt.Level = 1;
            };
            Action<Report, Report> middleConfig = (curNode, parentNode) =>
            {
                curNode.Level = parentNode.Level + 1;
            };
            Action<Report, int> footerConfig = (nd, initNum) =>
            {
                nd.Rgt = initNum;
            };

            Func<Report, IEnumerable<Report>> getChilds = (nd) => nd.Childs;

            return UnitRepository.CalaOrd(treeNode, headerConfig, middleConfig, footerConfig, getChilds, new NodeOrderInfo());
        }

        protected override void UpdateNodeOrder(Guid childId, Guid rootId)
        {
            var allNodes = UnitRepository.GetTreeNodes(childId.ToString(), rootId.ToString());
            var rootNode = allNodes.Find(nd => nd.Id == childId);

            if (rootNode != null)
            {
                CalaOrder(rootNode);
                UnitOfWork.SaveChanges();
            }
        }
        /// <summary>
        /// 创建或更新根结点
        /// </summary>
        /// <param name="rootReport">根结点</param>
        public void CreateOrUpdateRootNode(ref Report rootReport)
        {
            Func<Report, Report> getNewNode = rpt => new Report()
            {
                Parent = rpt,
                ParentId = rpt.Id,
                RootId = rpt.RootId
            };
            Action<Report, Report> getConfig = (rpt1, rpt2) =>
            {
                rpt2.Id = rpt1.Id;
                rpt2.RootId = rpt1.RootId;
            };
            Func<Report, Report, bool> getKey = (rpt1, rpt2) => rpt1.Id == rpt2.Id;

            Func<Report, List<Report>> getChilds = rpt => rpt.Childs;

            Func<Report, Guid> getCompareKey = rpt => rpt.Id;

            var nowReport = rootReport;

            if (nowReport.Id.Equals(DBNull.Value) || nowReport.Id == Guid.Empty)
            {
                var existRpt = new Report() { Parent = null, ParentId = null };
                UnitRepository.Upsert(ref existRpt);
                existRpt.RootId = existRpt.Id;
                UnitRepository.AddTreeNode(existRpt, nowReport, getChilds, getNewNode, getConfig);
                nowReport = existRpt;
            }
            else
            {
                var existReportList = UnitRepository.Query().Where(r => r.RootId == nowReport.Id).ToList();
                var existReport = existReportList.FirstOrDefault(r => r.Id == nowReport.Id && r.ParentId == null);
                nowReport.Parent = null;
                nowReport.ParentId = null;
                UnitRepository.UpdateTreeNode(existReport, nowReport, getChilds, getCompareKey, getKey, getNewNode, getConfig);
                nowReport = existReport;
            }
            CalaOrder(nowReport);
            UnitOfWork.SaveChanges();
            if (nowReport.RootId == Guid.Empty || nowReport.RootId.Equals(DBNull.Value))
            {
                // var retCount = UnitOfWork.ExecuteSqlCommand("update Reports set RootId = {1} where RootId ={0}", nowReport.RootId, nowReport.ReportId);
                //更新结点的相关信息
                UnitRepository.UpdateLocalNodeValues(nowReport, (nd) => nd.Childs, (nd) => nd.RootId = nowReport.Id);
                //保存更新,好像可以忽略,要测试才知道
                UnitOfWork.SaveChanges();
            }
        }
        /// <summary>
        /// 创建或更新子结点
        /// </summary>
        /// <param name="report">子结点</param>
        public void CreateOrUpdateChildNode(ref Report report)
        {
            if (report == null) throw new BadRequestException("请求对象为空.");
            if (report.ParentId == null || report.RootId.Equals(Guid.Empty))
            {
                throw new BadRequestException("请求对象父结点为空或请求对象的根结点ID为空.");
            }

            Func<Report, Report> getNewNode = rpt => new Report()
            {
                Parent = rpt,
                ParentId = rpt.Id,
                RootId = rpt.RootId
            };
            Action<Report, Report> getConfig = (rpt1, rpt2) =>
            {
                rpt2.Id = rpt1.Id;
                rpt2.RootId = rpt1.RootId;
            };
            Func<Report, Report, bool> getKey = (rpt1, rpt2) => rpt1.Id == rpt2.Id;
            Func<Report, List<Report>> getChilds = rpt => rpt.Childs;
            Func<Report, Guid> getCompareKey = rpt => rpt.Id;

            var nowReport = report;

            if (nowReport.Id == Guid.Empty)
            {
                var existRpt = new Report() { Parent = nowReport.Parent, ParentId = nowReport.Parent.ParentId, RootId = nowReport.RootId };
                UnitRepository.Upsert(ref existRpt);
                UnitRepository.AddTreeNode(existRpt, nowReport, getChilds, getNewNode, getConfig);
                nowReport = existRpt;
            }
            else
            {
                var existReportList = UnitRepository.GetTreeNodes(report.Id.ToString(), report.RootId.ToString());
                var existReport = existReportList.FirstOrDefault(r => r.Id == nowReport.Id);
                UnitRepository.UpdateTreeNode(existReport, nowReport, getChilds, getCompareKey, getKey, getNewNode, getConfig);
                nowReport = existReport;
            }
            UnitOfWork.SaveChanges();
            UpdateNodeOrder(nowReport.RootId, nowReport.RootId);
        }
        /// <summary>
        /// 删除结点
        /// </summary>
        /// <param name="report"></param>
        public void Remove(Report report)
        {
            var rptViewModel = report;
            var existNodeLists =
                UnitRepository.GetTreeNodes(rptViewModel.Id.ToString(), rptViewModel.RootId.ToString());

            var existNode = existNodeLists.Where(r => r.Id == rptViewModel.Id).FirstOrDefault();

            UnitRepository.DeleteTreeNode(existNode, (nd) => nd.Childs);
            UnitOfWork.SaveChanges();
            if (rptViewModel.RootId != rptViewModel.Id)
            {
                UpdateNodeOrder(rptViewModel.RootId, rptViewModel.RootId);
                UnitOfWork.SaveChanges();
            }
        }
        /// <summary>
        /// 获取结点
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public Report Select(Report report)
        {
            if (report == null) throw new BadRequestException("请求对象为空.");
            if (report.Id == Guid.Empty || report.Id.Equals(DBNull.Value)) throw new BadRequestException("请求对象ID为空.");

            var rptViewModel = report;
            var rpt = UnitRepository.GetTreeNodes(rptViewModel.Id.ToString(), rptViewModel.RootId.ToString());

            CalaOrder(rpt.FirstOrDefault(r => r.Id == rptViewModel.Id));
            var result = rpt.FirstOrDefault(r => r.Id == rptViewModel.Id);
            return result;
        }
        /// <summary>
        /// 获取结点
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public Report Select(Guid nodeId)
        {
            if (nodeId == Guid.Empty) throw new BadRequestException("指定的结点ID为空.");
            return Select(new Report { Id = nodeId });
        }
    }
}

using BusinessPlugins.BasicModule.Domain.Entities;
using InfrastructurePlugins.BaseModule.Domain.Services.Interfaces;
using System;

namespace BusinessPlugins.BasicModule.Domain.Services
{
    /// <summary>
    /// 报表管理
    /// </summary>
    public interface IReportManager : IDomainService<Report, Guid>
    {
        /// <summary>
        /// 创建或更新根结点
        /// </summary>
        /// <param name="rootReport">根结点</param>
        void CreateOrUpdateRootNode(ref Report rootReport);
        /// <summary>
        /// 创建或更新子结点
        /// </summary>
        /// <param name="report">子结点</param>
        void CreateOrUpdateChildNode(ref Report report);

        /// <summary>
        /// 删除结点
        /// </summary>
        /// <param name="report"></param>
        void Remove(Report report);

        /// <summary>
        /// 获取结点
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        Report Select(Report report);

        /// <summary>
        /// 获取结点
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Report Select(Guid nodeId);
    }
}

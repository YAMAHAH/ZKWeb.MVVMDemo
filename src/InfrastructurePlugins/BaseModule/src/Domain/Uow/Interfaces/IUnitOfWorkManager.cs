using System;

namespace InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces
{
    /// <summary>
    /// 工作单元管理
    /// </summary>
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// 获取当前的工作单元
        /// </summary>
        IActiveUnitOfWork Current { get; }

        /// <summary>
        /// 创建具有事务功能的工作单元,使用默认的工作单元选项
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IUnitOfWorkCompleteHandler Begin();
        /// <summary>
        /// 创建具有事务功能的工作单元
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IUnitOfWorkCompleteHandler Begin(UnitOfWorkOptions options);
        /// <summary>
        /// 创建不具有事务能力的工作单元，嵌套时支持根据选项创建不同的工作单元
        /// </summary>
        /// <param name="forceNewScope"></param>
        /// <returns></returns>
        IDisposable CreateUnitOfWork(bool forceNewScope = false);

        /// <summary>
        /// 创建具有自动提交事务能力的工作单元
        /// </summary>
        /// <param name="postAction"></param>
        /// <param name="unitOfWorkOptions"></param>
        void CreateUnitOfWorkScope(Action postAction, UnitOfWorkOptions unitOfWorkOptions = null);
    }
}

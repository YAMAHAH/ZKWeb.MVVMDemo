using System;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces
{
    /// <summary>
    /// Unit of work manager.
    /// Used to begin and control a unit of work.
    /// </summary>
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// Gets currently active unit of work (or null if not exists).
        /// </summary>
        IActiveUnitOfWork Current { get; }

        /// <summary>
        /// Begins a new unit of work.
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work</returns>
        IUnitOfWorkCompleteHandler Begin();

        /// <summary>
        /// Begins a new unit of work.
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work</returns>
        //IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope);

        /// <summary>
        /// Begins a new unit of work.
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work</returns>
        IUnitOfWorkCompleteHandler Begin(UnitOfWorkOptions options);

        IDisposable CreateUnitOfWork(bool forceNewScope = false);

        void CreateUnitOfWorkScope(Action postAction, UnitOfWorkOptions unitOfWorkOptions = null);
    }
}

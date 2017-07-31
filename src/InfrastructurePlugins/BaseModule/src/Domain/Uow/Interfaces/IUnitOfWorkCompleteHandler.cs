using System;
using System.Threading.Tasks;

namespace InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces
{
    /// <summary>
    /// 工作单元事务完成处理
    /// </summary>
    public interface IUnitOfWorkCompleteHandler : IDisposable
    {
        /// <summary>
        /// 保存并提交事务
        /// </summary>
        void Complete();

        /// <summary>
        /// 异步保存并提交事务
        /// </summary>
        /// <returns></returns>
        Task CompleteAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructurePlugins.BaseModule.Domain.Uow.Interfaces
{

    /// <summary>
    /// This interface is used to work with active unit of work.
    /// This interface can not be injected.
    /// Use <see cref="IUnitOfWorkManager"/> instead.
    /// </summary>
    public interface IActiveUnitOfWork
    {
        /// <summary>
        /// Gets if this unit of work is transactional.
        /// </summary>
        UnitOfWorkOptions Options { get; }

        /// <summary>
        /// Gets data filter configurations for this unit of work.
        /// </summary>
        //IReadOnlyList<DataFilterConfiguration> Filters { get; }

        /// <summary>
        /// Saves all changes until now in this unit of work.
        /// This method may be called to apply changes whenever needed.
        /// Note that if this unit of work is transactional, saved changes are also rolled back if transaction is rolled back.
        /// No explicit call is needed to SaveChanges generally, 
        /// since all changes saved at end of a unit of work automatically.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Saves all changes until now in this unit of work.
        /// This method may be called to apply changes whenever needed.
        /// Note that if this unit of work is transactional, saved changes are also rolled back if transaction is rolled back.
        /// No explicit call is needed to SaveChanges generally, 
        /// since all changes saved at end of a unit of work automatically.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// 在一定范围内禁用指定的过滤器
        /// </summary>
        /// <param name="filterNames">过滤器列表</param>
        /// <returns></returns>
        IDisposable DisableFilter(params string[] filterNames);

        /// <summary>
        /// 在一定范围内启用指定名称的过滤器
        /// </summary>
        /// <param name="filterNames">过滤名称列表</param>
        /// <returns></returns>
        IDisposable EnableFilter(params string[] filterNames);

        /// <summary>
        /// Checks if a filter is enabled or not.
        /// </summary>
        /// <param name="filterName">Name of the filter. <see cref="AbpDataFilters"/> for standard filters.</param>
        //bool IsFilterEnabled(string filterName);

        /// <summary>
        /// Sets (overrides) value of a filter parameter.
        /// </summary>
        /// <param name="filterName">Name of the filter</param>
        /// <param name="parameterName">Parameter's name</param>
        /// <param name="value">Value of the parameter to be set</param>
        //IDisposable SetFilterParameter(string filterName, string parameterName, object value);

    }
}

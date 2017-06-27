using System;
using System.Data;

namespace InfrastructurePlugins.BaseModule.Domain.Uow
{
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// Is this UOW transactional?
        /// Uses default value if not supplied.
        /// </summary>
        public bool IsTransactional { get; set; }

        public bool IsStandalone { get; set; }
        /// <summary>
        /// Timeout of UOW As milliseconds.
        /// Uses default value if not supplied.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// If this UOW is transactional, this option indicated the isolation level of the transaction.
        /// Uses default value if not supplied.
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }
    }
}

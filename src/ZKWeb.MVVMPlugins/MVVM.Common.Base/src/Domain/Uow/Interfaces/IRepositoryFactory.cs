﻿using System;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Database;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces
{
    /// <summary>
    /// Defines the interfaces for <see cref="IRepository{TEntity}"/> interfaces.
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
        IUnitOfWorkRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>, new();
    }
}

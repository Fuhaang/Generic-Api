using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Contract;
using System;

namespace UnitOfWork.Contract
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        /// <summary>
        /// Gets the db context.
        /// </summary>
        /// <returns>The instance of type TContext.</returns>
        TContext DbContext { get; }

        /// <summary>
        /// Gets the specified repository for the TEntity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from GenericRepository interface.</returns>
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity;
    }
}

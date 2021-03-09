using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Contract;
using System;
using System.Collections.Generic;
using UnitOfWork.Contract;

namespace UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private bool disposed = false;
        private Dictionary<Type, object> _repositories;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork<TContext>.
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TContext DbContext => _context;

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<TEntity>(_context);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                if (_repositories != null)
                {
                    _repositories.Clear();
                }
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}

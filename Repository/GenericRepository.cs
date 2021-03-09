using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the GenericRepository<TEntity>.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        #region CREATE
        public async virtual Task<bool> Add(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                var created = await _dbContext.SaveChangesAsync();

                return created > 0;
            }
            catch
            {
                return false;
            }
        }

        public async virtual Task<bool> Add(IEnumerable<TEntity> entities)
        {
            try
            {
                _dbSet.AddRange(entities);
                var created = await _dbContext.SaveChangesAsync();

                return created > 0;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region READ
        public async virtual Task<TEntity> GetById(long id)
        {
            return await _dbSet.FirstAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        private IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }

        public async virtual Task<TEntity> GetFirstOrDefault(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true
        )
        {
            IQueryable<TEntity> query = Get(predicate, include, disableTracking);

            if (orderBy != null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }
            else
            {
                return await query.FirstOrDefaultAsync();
            }
        }

        public async virtual Task<IEnumerable<TEntity>> GetMuliple(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true
        )
        {
            IQueryable<TEntity> query = Get(predicate, include, disableTracking);

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }
        #endregion

        #region UPDATE
        public async virtual Task<bool> Update(TEntity entity)
        {
            if (entity == null)
                return false;
            if (entity.Id == 0)
                return false;
            try
            {
                _dbSet.Update(entity);
                var updated = await _dbContext.SaveChangesAsync();

                return updated > 0;
            }
            catch
            {
                return false;
            }
        }

        public async virtual Task<bool> Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                return false;
            if (!entities.All(e => e.Id != 0))
                return false;
            try
            {
                _dbSet.UpdateRange(entities);
                var updated = await _dbContext.SaveChangesAsync();

                return updated > 0;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DELETE
        public async virtual Task<bool> Delete(long id)
        {
            var entityToDelete = await _dbSet.SingleOrDefaultAsync(t => t.Id == id);
            if (entityToDelete == null)
            {
                return false;
            }

            _dbSet.Remove(entityToDelete);
            var deleted = await _dbContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async virtual Task<bool> Delete(TEntity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
            var deleted = await _dbContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async virtual Task<bool> Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                return false;
            }

            _dbSet.RemoveRange(entities);
            var deleted = await _dbContext.SaveChangesAsync();

            return deleted > 0;
        }
        #endregion

        #region OTHER
        public async virtual Task<int> Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.CountAsync();
            }
            else
            {
                return await _dbSet.CountAsync(predicate);
            }
        }

        public async virtual Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
        #endregion
    }
}

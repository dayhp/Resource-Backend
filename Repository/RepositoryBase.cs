using Entities;
using HumanResourceAPI.Infrastrcuture.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public class RepositoryBase<TEntity, K> : IRepositoryBase<TEntity, K>
        where TEntity : DomainEntity<K>
    {
        private readonly AppDbContext _dbContext;
        public RepositoryBase(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> FindAll(bool trackChange, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            if (!trackChange)

            {
                query = query.AsNoTracking();
            }
            if (includeProperties == null || includeProperties.Length == 0) return query;
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<TEntity> FindAll(bool trackChange, Expression<Func<TEntity, bool>>[] predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            if (!trackChange)
            {
                query = query.AsNoTracking();
            }
            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            if (predicate != null)
            {
                foreach (var pred in predicate)
                {
                    query = query.Where(pred);
                }
            }
            return query;
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public TEntity FindById(K id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            //return FindAll(false, includeProperties).SingleOrDefault(e => EF.Property<K>(e, "Id").Equals(id));
            return FindAll(false, includeProperties).SingleOrDefault(e => e.Id.Equals(id));
        }

        public async Task<TEntity> FindByIdAsync(K id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await FindAll(false, includeProperties).SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        public TEntity FindSingle(bool trackChange, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return FindAll(false, includeProperties).SingleOrDefault(predicate);
        }

        public async Task<TEntity> FindSingleAsync(bool trackChange, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await FindAll(false, includeProperties).SingleOrDefaultAsync(predicate);
        }
        public void Create(TEntity entity) => _dbContext.Set<TEntity>().Add(entity);
        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);
    }
}

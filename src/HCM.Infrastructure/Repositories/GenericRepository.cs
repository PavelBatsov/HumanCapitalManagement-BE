using HCM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HCM.Infrastructure.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly AppDbContext dbContext;

        public GenericRepository(AppDbContext dbContext)
            => this.dbContext = dbContext;

        public virtual async Task<TEntity> GetAsync(Guid id)
            => await dbContext.Set<TEntity>().FindAsync(id);

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
            => await dbContext.Set<TEntity>().ToListAsync();

        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
            => dbContext.Set<TEntity>().Where(predicate);

        public virtual TEntity Add(TEntity entity)
            => dbContext.Set<TEntity>().Add(entity).Entity;

        public virtual async Task AddAsync(TEntity entity)
            => await dbContext.Set<TEntity>().AddAsync(entity);

        public virtual TEntity Update(TEntity entity)
            => dbContext.Set<TEntity>().Update(entity).Entity;

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
            => await dbContext.Set<TEntity>().AddRangeAsync(entities);

        public virtual void Delete(TEntity entity)
            => dbContext.Set<TEntity>().Remove(entity);

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
            => dbContext.Set<TEntity>().RemoveRange(entities);

        public virtual void Save()
            => dbContext.SaveChanges();

        public virtual async Task SaveAsync()
            => await dbContext.SaveChangesAsync();
    }
}

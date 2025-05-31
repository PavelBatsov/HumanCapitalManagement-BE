using HCM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HCM.Infrastructure.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly AppDbContext context;

        public GenericRepository(AppDbContext context)
            => this.context = context;

        public virtual async Task<TEntity> GetAsync(Guid id)
            => await context.Set<TEntity>().FindAsync(id);

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
            => await context.Set<TEntity>().ToListAsync();

        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
            => context.Set<TEntity>().Where(predicate);

        public virtual TEntity Add(TEntity entity)
            => context.Set<TEntity>().Add(entity).Entity;

        public virtual async Task AddAsync(TEntity entity)
            => await context.Set<TEntity>().AddAsync(entity);

        public virtual TEntity Update(TEntity entity)
            => context.Set<TEntity>().Update(entity).Entity;

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
            => await context.Set<TEntity>().AddRangeAsync(entities);

        public virtual void Delete(TEntity entity)
            => context.Set<TEntity>().Remove(entity);

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
            => context.Set<TEntity>().RemoveRange(entities);

        public virtual void Save()
            => context.SaveChanges();

        public virtual async Task SaveAsync()
            => await context.SaveChangesAsync();
    }
}

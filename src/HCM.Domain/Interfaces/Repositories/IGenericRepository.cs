using System.Linq.Expressions;

namespace HCM.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> GetAsync(Guid id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        Task AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);

        void Save();

        Task SaveAsync();
    }
}

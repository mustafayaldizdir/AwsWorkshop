using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Application.Abstracts.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, object>> includeProperty);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, object>> includeProperty1, Expression<Func<TEntity, object>> includeProperty2);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, object>> includeProperty1, Expression<Func<TEntity, object>> includeProperty2, Expression<Func<TEntity, object>> includeProperty3);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
        TEntity Update(TEntity entity);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task SaveAsync();
    }
}

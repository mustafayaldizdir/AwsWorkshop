using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AwsWorkshop.Product.Api.Core.Application.Dtos;

namespace AwsWorkshop.Product.Api.Core.Application.Abstracts
{
    public interface IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<Response<TDto>> GetByIdAsync(int id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, object>> includeProperty);
        Task<Response<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, object>> includeProperty1, Expression<Func<TEntity, object>> includeProperty2);
        Task<Response<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, object>> includeProperty1, Expression<Func<TEntity, object>> includeProperty2, Expression<Func<TEntity, object>> includeProperty3);
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);
        Task<Response<TDto>> AddAsync(TDto entity);
        Task<Response<TDto>> AddAndSaveAsync(TDto entity);
        Task<Response<NoContent>> Remove(int id);
        Task<Response<NoContent>> Update(TDto entity);
        Task<Response<TDto>> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<Response<List<TDto>>> AddRangeAsync(List<TDto> entities);
        Task<Response<bool>> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<Response<NoContent>> SaveAsync();
    }
}

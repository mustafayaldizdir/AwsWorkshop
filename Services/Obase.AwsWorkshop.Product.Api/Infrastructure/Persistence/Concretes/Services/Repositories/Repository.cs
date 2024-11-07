using Microsoft.EntityFrameworkCore;
using AwsWorkshop.Product.Api.Core.Application.Abstracts.Repositories;
using AwsWorkshop.Product.Api.Core.Application.Abstracts.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Product.Api.Infrastructure.Persistence.Concretes.Services.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IUnitOfWork _unitOfWork;

        public Repository(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(T entity)
        {
            var data = await _dbSet.AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var list = await _dbSet.ToListAsync();
            return list;
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>> includeProperty)
        {
            var list = await _dbSet.Include(includeProperty).ToListAsync();
            return list;
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>> includeProperty1, Expression<Func<T, object>> includeProperty2)
        {
            var list = await _dbSet.Include(includeProperty1).Include(includeProperty2).ToListAsync();
            return list;
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>> includeProperty1, Expression<Func<T, object>> includeProperty2, Expression<Func<T, object>> includeProperty3)
        {
            var list = await _dbSet.Include(includeProperty1).Include(includeProperty2).Include(includeProperty3).ToListAsync();
            return list;
        }
        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
        public T Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await _dbSet.FirstOrDefaultAsync(predicate);
            return result;
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await _dbSet.AnyAsync(predicate);
            return result;
        }
        public async Task SaveAsync()
        {
            await _unitOfWork.CommitAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using AwsWorkshop.Product.Api.Core.Application.Abstracts.UnitOfWorks;

namespace AwsWorkshop.Product.Api.Infrastructure.Persistence.Concretes.Services.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

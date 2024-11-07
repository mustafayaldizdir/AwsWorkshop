

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AwsWorkshop.Product.Api.Core.Domain.Entities;
using System.Text.Json;

namespace AwsWorkshop.Product.Api.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApplicationDbContext> _logger;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions, IConfiguration configuration, ILogger<ApplicationDbContext> logger) : base(dbContextOptions)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public DbSet<AppProduct> AppProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }

    }
}

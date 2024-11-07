using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AwsWorkshop.Domain.Configurations;
using AwsWorkshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace AwsWorkshop.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<UserApp, UserAppRole, int>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApplicationDbContext> _logger;
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> dbContextOptions, 
            IConfiguration configuration, 
            ILogger<ApplicationDbContext> logger) : base(dbContextOptions)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(UserRefreshTokenConfiguration).Assembly);
            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}

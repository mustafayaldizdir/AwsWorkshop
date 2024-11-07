using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AwsWorkshop.Product.Api.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static async Task AddPersistenceService(this IServiceCollection collection)
        {
            using var provider = collection.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();
            var logger = provider.GetRequiredService<ILogger<ApplicationDbContext>>();
            try
            {
                collection.AddDbContext<ApplicationDbContext>(option => option.UseNpgsql(e => e.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

                using IServiceScope scope = collection.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.SetConnectionString(configuration.GetConnectionString("DefaultConnection"));

                if (dbContext.Database.GetMigrations().Count() > 0)
                {
                    await dbContext.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

        }
    }
}

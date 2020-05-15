using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PaymentServiceSample.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class PaymentServiceSampleMigrationsDbContextFactory : IDesignTimeDbContextFactory<PaymentServiceSampleMigrationsDbContext>
    {
        public PaymentServiceSampleMigrationsDbContext CreateDbContext(string[] args)
        {
            PaymentServiceSampleEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<PaymentServiceSampleMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new PaymentServiceSampleMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}

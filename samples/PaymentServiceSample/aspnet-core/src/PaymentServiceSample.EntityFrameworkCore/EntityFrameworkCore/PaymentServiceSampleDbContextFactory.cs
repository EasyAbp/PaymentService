using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PaymentServiceSample.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class PaymentServiceSampleDbContextFactory : IDesignTimeDbContextFactory<PaymentServiceSampleDbContext>
    {
        public PaymentServiceSampleDbContext CreateDbContext(string[] args)
        {
            PaymentServiceSampleEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<PaymentServiceSampleDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new PaymentServiceSampleDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PaymentServiceSample.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}

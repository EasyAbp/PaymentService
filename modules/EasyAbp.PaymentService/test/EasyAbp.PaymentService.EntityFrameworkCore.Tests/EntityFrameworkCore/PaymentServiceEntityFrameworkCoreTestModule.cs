using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreSqliteModule),
        typeof(PaymentServiceTestBaseModule),
        typeof(PaymentServiceEntityFrameworkCoreModule)
    )]
    public class PaymentServiceEntityFrameworkCoreTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysDisableUnitOfWorkTransaction();
            var sqliteConnection = CreateDatabaseAndGetConnection();

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(abpDbContextConfigurationContext =>
                {
                    abpDbContextConfigurationContext.DbContextOptions.UseSqlite(sqliteConnection);
                });
            });
        }

        private static SqliteConnection CreateDatabaseAndGetConnection()
        {
            var connection = new AbpUnitTestSqliteConnection("Data Source=:memory:");
            connection.Open();

            new PaymentServiceDbContext(
                new DbContextOptionsBuilder<PaymentServiceDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();

            return connection;
        }
    }
}
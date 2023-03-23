using EasyAbp.PaymentService.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Prepayment.EntityFrameworkCore
{
    [DependsOn(
        typeof(PrepaymentTestBaseModule),
        typeof(PaymentServiceEntityFrameworkCoreModule),
        typeof(PaymentServicePrepaymentEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
    public class PrepaymentEntityFrameworkCoreTestModule : AbpModule
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
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            new PaymentServiceDbContext(
                new DbContextOptionsBuilder<PaymentServiceDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();
            
            new PaymentServicePrepaymentDbContext(
                new DbContextOptionsBuilder<PaymentServicePrepaymentDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();
            
            return connection;
        }
    }
}

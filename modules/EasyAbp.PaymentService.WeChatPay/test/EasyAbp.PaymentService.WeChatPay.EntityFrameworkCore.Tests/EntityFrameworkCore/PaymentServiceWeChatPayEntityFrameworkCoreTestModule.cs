using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore
{
    [DependsOn(
        typeof(PaymentServiceWeChatPayTestBaseModule),
        typeof(PaymentServiceWeChatPayEntityFrameworkCoreModule)
        )]
    public class PaymentServiceWeChatPayEntityFrameworkCoreTestModule : AbpModule
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

            new PaymentServiceWeChatPayDbContext(
                new DbContextOptionsBuilder<PaymentServiceWeChatPayDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();
            
            return connection;
        }
    }
}

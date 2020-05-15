using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentServiceSample.Data;
using Volo.Abp.DependencyInjection;

namespace PaymentServiceSample.EntityFrameworkCore
{
    public class EntityFrameworkCorePaymentServiceSampleDbSchemaMigrator
        : IPaymentServiceSampleDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCorePaymentServiceSampleDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the PaymentServiceSampleMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<PaymentServiceSampleMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
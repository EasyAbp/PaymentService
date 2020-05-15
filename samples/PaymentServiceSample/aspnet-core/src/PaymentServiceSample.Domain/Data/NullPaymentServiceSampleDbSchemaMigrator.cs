using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PaymentServiceSample.Data
{
    /* This is used if database provider does't define
     * IPaymentServiceSampleDbSchemaMigrator implementation.
     */
    public class NullPaymentServiceSampleDbSchemaMigrator : IPaymentServiceSampleDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}
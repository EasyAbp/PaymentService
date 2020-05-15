using System.Threading.Tasks;

namespace PaymentServiceSample.Data
{
    public interface IPaymentServiceSampleDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}

using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace EasyAbp.PaymentService;

public class PaymentServiceDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;

    public PaymentServiceDataSeedContributor(
        IGuidGenerator guidGenerator)
    {
        _guidGenerator = guidGenerator;
    }
        
    public Task SeedAsync(DataSeedContext context)
    {
        /* Instead of returning the Task.CompletedTask, you can insert your test data
         * at this point!
         */

        return Task.CompletedTask;
    }
}
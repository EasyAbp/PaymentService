using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.PaymentService.WeChatPay.Background;

public class EmptyJob : IAsyncBackgroundJob<EmptyJobArgs>, ITransientDependency
{
    public virtual Task ExecuteAsync(EmptyJobArgs args)
    {
        return Task.CompletedTask;
    }
}
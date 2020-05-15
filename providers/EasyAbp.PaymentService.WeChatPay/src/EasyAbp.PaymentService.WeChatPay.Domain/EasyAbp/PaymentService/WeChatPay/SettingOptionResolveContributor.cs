using System;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay;
using EasyAbp.Abp.WeChat.Pay.Infrastructure.OptionResolve;
using EasyAbp.PaymentService.WeChatPay.Settings;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Settings;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class SettingOptionResolveContributor : IWeChatPayOptionResolveContributor
    {
        public const string ContributorName = "Setting";

        public string Name => ContributorName;

        public virtual async Task ResolveAsync(WeChatPayOptionsResolverContext context)
        {
            var settingProvider = context.ServiceProvider.GetRequiredService<ISettingProvider>();
            context.Options = new AbpWeChatPayOptions
            {
                ApiKey = await settingProvider.GetOrNullAsync(WeChatPaySettings.ApiKey),
                MchId = await settingProvider.GetOrNullAsync(WeChatPaySettings.MchId),
                IsSandBox = Convert.ToBoolean(await settingProvider.GetOrNullAsync(WeChatPaySettings.IsSandBox)),
                NotifyUrl = await settingProvider.GetOrNullAsync(WeChatPaySettings.NotifyUrl),
                RefundNotifyUrl = await settingProvider.GetOrNullAsync(WeChatPaySettings.RefundNotifyUrl),
                CertificatePath = await settingProvider.GetOrNullAsync(WeChatPaySettings.CertificatePath),
                CertificateSecret = await settingProvider.GetOrNullAsync(WeChatPaySettings.CertificateSecret)
            };
        }
    }
}
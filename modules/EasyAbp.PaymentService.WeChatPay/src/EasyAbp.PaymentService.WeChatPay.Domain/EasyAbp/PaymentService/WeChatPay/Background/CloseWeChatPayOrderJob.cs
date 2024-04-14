using System;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Services;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.Models;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.WeChatPay.Background;

public class CloseWeChatPayOrderJob : IAsyncBackgroundJob<CloseWeChatPayOrderJobArgs>, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IAbpWeChatPayServiceFactory _abpWeChatPayServiceFactory;

    public CloseWeChatPayOrderJob(
        ICurrentTenant currentTenant,
        IAbpWeChatPayServiceFactory abpWeChatPayServiceFactory)
    {
        _currentTenant = currentTenant;
        _abpWeChatPayServiceFactory = abpWeChatPayServiceFactory;
    }

    public virtual async Task ExecuteAsync(CloseWeChatPayOrderJobArgs args)
    {
        using var change = _currentTenant.Change(args.TenantId);

        var basicPaymentService =
            await _abpWeChatPayServiceFactory.CreateAsync<BasicPaymentService>(args.MchId);

        var response = await basicPaymentService.CloseOrderAsync(
            new CloseOrderRequest
            {
                MchId = args.MchId,
                OutTradeNo = args.OutTradeNo
            });

        // ignore the "ORDERCLOSED" status.
        if (!response.Code.IsNullOrEmpty() && response.Code != "ORDERCLOSED")
        {
            throw new WeChatPayBusinessErrorException(args.OutTradeNo, response.Code, response.Message);
        }
    }
}
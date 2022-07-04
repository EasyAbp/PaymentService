using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Services.Pay;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.PaymentService.WeChatPay;

public class CloseWeChatPayOrderJob : IAsyncBackgroundJob<CloseWeChatPayOrderJobArgs>, ITransientDependency
{
    private readonly ServiceProviderPayService _serviceProviderPayService;

    public CloseWeChatPayOrderJob(ServiceProviderPayService serviceProviderPayService)
    {
        _serviceProviderPayService = serviceProviderPayService;
    }
    
    public virtual async Task ExecuteAsync(CloseWeChatPayOrderJobArgs args)
    {
        var orderQueryResult = await _serviceProviderPayService.CloseOrderAsync(
            appId: args.AppId,
            mchId: args.MchId,
            subAppId: null,
            subMchId: null,
            outTradeNo: args.OutTradeNo
        );
            
        var dict = orderQueryResult.SelectSingleNode("xml").ToDictionary() ?? throw new NullReferenceException();
        var resultCode = dict.GetOrDefault("result_code");
        var errCode = dict.GetOrDefault("err_code");
        var errCodeDes = dict.GetOrDefault("err_code_des");

        // ignore the "ORDERCLOSED" status.
        if (resultCode != "SUCCESS" && errCode != "ORDERCLOSED")
        {
            throw new WeChatPayBusinessErrorException(args.OutTradeNo, errCode, errCodeDes);
        }
    }
}
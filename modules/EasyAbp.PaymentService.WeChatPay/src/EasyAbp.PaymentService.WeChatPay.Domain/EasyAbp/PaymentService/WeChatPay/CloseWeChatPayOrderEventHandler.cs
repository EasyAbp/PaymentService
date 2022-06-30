using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.Internal;
using EasyAbp.Abp.WeChat.Pay.Services.Pay;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace EasyAbp.PaymentService.WeChatPay;

public class CloseWeChatPayOrderEventHandler : IDistributedEventHandler<CloseWeChatPayOrderEto>, ITransientDependency
{
    private readonly ServiceProviderPayService _serviceProviderPayService;

    public CloseWeChatPayOrderEventHandler(ServiceProviderPayService serviceProviderPayService)
    {
        _serviceProviderPayService = serviceProviderPayService;
    }
    
    public virtual async Task HandleEventAsync(CloseWeChatPayOrderEto eventData)
    {
        var orderQueryResult = await _serviceProviderPayService.CloseOrderAsync(
            appId: eventData.AppId,
            mchId: eventData.MchId,
            subAppId: null,
            subMchId: null,
            outTradeNo: eventData.OutTradeNo
        );
            
        var dict = orderQueryResult.SelectSingleNode("xml").ToDictionary() ?? throw new NullReferenceException();
        var resultCode = dict.GetOrDefault("result_code");
        var errCode = dict.GetOrDefault("err_code");
        var errCodeDes = dict.GetOrDefault("err_code_des");

        // ignore the "ORDERCLOSED" status.
        if (resultCode != "SUCCESS" && errCode != "ORDERCLOSED")
        {
            throw new WeChatPayBusinessErrorException(eventData.OutTradeNo, errCode, errCodeDes);
        }
    }
}
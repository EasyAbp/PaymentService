using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Payments.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace EasyAbp.PaymentService.WeChatPay.AutoMapper;

public class AutoMapperTests : WeChatPayApplicationTestBase
{
    [Fact]
    public async Task Should_Map_Payment_ExtraProperties()
    {
        var objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();

        var payment = new Payment(Guid.NewGuid(), null, Guid.NewGuid(), "Free", "CNY", 0m, new List<PaymentItem>());
        
        payment.SetProperty("appid", "myappid");
        payment.SetProperty("trade_type", "mytradetype");
        payment.SetProperty("prepay_id", "myprepayid");
        payment.SetProperty("code_url", "mycodeurl");
        
        var dto = objectMapper.Map<Payment, PaymentDto>(payment);
        
        dto.GetProperty<string>("appid").ShouldBe("myappid");
        dto.GetProperty<string>("trade_type").ShouldBe("mytradetype");
        dto.GetProperty<string>("prepay_id").ShouldBe("myprepayid");
        dto.GetProperty<string>("code_url").ShouldBe("mycodeurl");
    }
}
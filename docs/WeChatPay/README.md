# PaymentService.WeChatPay

[![NuGet](https://img.shields.io/nuget/v/EasyAbp.PaymentService.WeChatPay.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.WeChatPay.Domain.Shared)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.PaymentService.WeChatPay.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.WeChatPay.Domain.Shared)

WeChatPay implementation of the EasyAbp.PaymentService module.

## Getting Started

> Should also install the [PaymentService module](../../README.md#getting-started) since this module depends on it.

* Install with [AbpHelper](https://github.com/EasyAbp/AbpHelper.GUI)

    Coming soon.

1. Install the following NuGet packages. (see how)

    * EasyAbp.PaymentService.WeChatPay.Application
    * EasyAbp.PaymentService.WeChatPay.Application.Contracts
    * EasyAbp.PaymentService.WeChatPay.Domain
    * EasyAbp.PaymentService.WeChatPay.Domain.Shared
    * EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore
    * EasyAbp.PaymentService.WeChatPay.HttpApi
    * EasyAbp.PaymentService.WeChatPay.HttpApi.Client
    * (Optional) EasyAbp.PaymentService.WeChatPay.MongoDB
    * (Optional) EasyAbp.PaymentService.WeChatPay.Web

1. Add `DependsOn(typeof(xxx))` attribute to configure the module dependencies. (see how)

1. Add `builder.ConfigurePaymentServiceWeChatPay();` to the `OnModelCreating()` method in **MyProjectMigrationsDbContext.cs**.

1. Add EF Core migrations and update your database. See: [ABP document](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC#add-new-migration-update-the-database).

## Usage

1. Register the WeChatPay payment method:
    ```csharp
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var resolver = context.ServiceProvider.GetRequiredService<IPaymentServiceResolver>();

        resolver.TryRegisterProvider(WeChatPayPaymentServiceProvider.PaymentMethod, typeof(WeChatPayPaymentServiceProvider));
    }
    ```
    
2. Configure the [WeChatPay settings](../../modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Domain/EasyAbp/PaymentService/WeChatPay/Settings/WeChatPaySettings.cs), you can try to use [SettingUI](https://github.com/EasyAbp/Abp.SettingUi) to finish this work.

    > See the [demo](../../samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Web/appsettings.json), it is also according to the [document](https://github.com/EasyAbp/Abp.WeChat/blob/master/doc/WeChatPay.md) of the EasyAbp.Abp.WeChat module.

3. Create a payment and handle the payment completed event: **(skip this part if you are using [EasyAbp.EShop](https://github.com/EasyAbp/EShop))**

    1. Create a payment with the payment method `WeChatPay` (other modules or apps that depend on PaymentService module should create payments via distributed events):
    ```csharp
    await _distributedEventBus.PublishAsync(new CreatePaymentEto
    {
        TenantId = CurrentTenant.Id,
        UserId = CurrentUser.GetId(),
        PaymentMethod = "WeChatPay",   // Should specify the payment method as "WeChatPay"
        Currency = "CNY",   // Should always be "CNY"
        PaymentItems = orders.Select(order => new CreatePaymentItemEto
        {
            ItemType = "MyCustomKeyword", // It is just a sample and you can customize it yourself
            ItemKey = order.Id,
            Currency = "CNY",
            OriginalPaymentAmount = order.Price
        }).ToList()
    });
    ```
    > please refer to the [usage in EShop](https://github.com/EasyAbp/EShop/blob/dev/modules/EasyAbp.EShop.Payments/src/EasyAbp.EShop.Payments.Application/EasyAbp/EShop/Payments/Payments/PaymentAppService.cs)
    
    2. Handle the payment completed distributed event:
    ```csharp
    public class MyCustomPaymentCompletedEventHandler : IDistributedEventHandler<PaymentCompletedEto>, ITransientDependency
    {
        [UnitOfWork(isTransactional: true)]
        public virtual async Task HandleEventAsync(PaymentCompletedEto eventData)
        {
            foreach (var item in payment.PaymentItems.Where(item => item.ItemType == "MyCustomKeyword"))
            {
                // Maybe you can automatically send out the goods to the customer here.
            }
        }
    }
    ```
    > please refer to the [usage in EShop](https://github.com/EasyAbp/EShop/blob/dev/modules/EasyAbp.EShop.Orders/src/EasyAbp.EShop.Orders.Domain/EasyAbp/EShop/Orders/Orders/OrderPaymentCompletedEventHandler.cs)

4. Users can use the API `/api/paymentService/payment/{id}/pay` to finish the payment, please put the necessary params in the `ExtraProperties`:
```
{
    "extraProperties": {
        "trade_type": "JSAPI",
        "appid": "wx81a2956875268fk8"   // You can specify an appid or get it from the input from the client.
    }
}
```

## Roadmap

- [ ] Unit tests.

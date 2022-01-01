# PaymentService.WeChatPay

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2FEasyAbp%2FPaymentService%2Fmaster%2FDirectory.Build.props)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/EasyAbp.PaymentService.WeChatPay.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.WeChatPay.Domain.Shared)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.PaymentService.WeChatPay.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.WeChatPay.Domain.Shared)
[![Discord online](https://badgen.net/discord/online-members/S6QaezrCRq?label=Discord)](https://discord.gg/S6QaezrCRq)
[![GitHub stars](https://img.shields.io/github/stars/EasyAbp/PaymentService?style=social)](https://www.github.com/EasyAbp/PaymentService)

WeChatPay implementation of the EasyAbp.PaymentService module.

## Online Demo

We have launched an online demo for this module: [https://pay.samples.easyabp.io](https://pay.samples.easyabp.io)

## Installation

> Should also install the [PaymentService module](/docs/README.md#getting-started) since this module depends on it.

1. Install the following NuGet packages. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/How-To.md#add-nuget-packages))

    * EasyAbp.PaymentService.WeChatPay.Application
    * EasyAbp.PaymentService.WeChatPay.Application.Contracts
    * EasyAbp.PaymentService.WeChatPay.Domain
    * EasyAbp.PaymentService.WeChatPay.Domain.Shared
    * EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore
    * EasyAbp.PaymentService.WeChatPay.HttpApi
    * EasyAbp.PaymentService.WeChatPay.HttpApi.Client
    * (Optional) EasyAbp.PaymentService.WeChatPay.MongoDB
    * (Optional) EasyAbp.PaymentService.WeChatPay.Web

1. Add `DependsOn(typeof(PaymentServiceWeChatPayXxxModule))` attribute to configure the module dependencies. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/How-To.md#add-module-dependencies))

1. Add `builder.ConfigurePaymentServiceWeChatPay();` to the `OnModelCreating()` method in **MyProjectMigrationsDbContext.cs**.

1. Add EF Core migrations and update your database. See: [ABP document](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC#add-new-migration-update-the-database).

## Usage

1. Register the WeChatPay payment method:
    ```csharp
    Configure<PaymentServiceOptions>(options =>
    {
        options.Providers.Configure<WeChatPayPaymentServiceProvider>(WeChatPayPaymentServiceProvider.PaymentMethod);
    });
    ```
    
2. Configure the [WeChatPay settings](https://github.com/EasyAbp/PaymentService/blob/master/modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Domain/EasyAbp/PaymentService/WeChatPay/Settings/WeChatPaySettings.cs), you can try to use [SettingUI](https://github.com/EasyAbp/Abp.SettingUi) to finish this work.
    > See the [demo](https://github.com/EasyAbp/PaymentService/blob/master/samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Web/appsettings.json), it is also according to the [document](https://github.com/EasyAbp/Abp.WeChat/blob/master/doc/WeChatPay.md) of the EasyAbp.Abp.WeChat module.

3. Pay with WeChatPay.
    1. Users can use the API `/api/paymentService/payment/{id}/pay` to finish the payment, please put the necessary params in the `ExtraProperties`:
    
        ```
        {
            "extraProperties": {
                "trade_type": "JSAPI",
                "appid": "wx81a2956875268fk8"   // You can specify an appid or get it from the input from the client.
            }
        }
        ```

    > Skip the following steps if you are using the [EasyAbp.EShop](https://github.com/EasyAbp/EShop).

    <details>
    <summary>See more steps</summary>

    2. Create a payment with the payment method `WeChatPay`.
        > Other modules or apps that depend on PaymentService module should create payments via distributed events.

        <details>
        <summary>See sample code</summary>

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
                OriginalPaymentAmount = order.Price
            }).ToList()
        });
        ```
        > please refer to the [usage in EShop](https://github.com/EasyAbp/EShop/blob/dev/modules/EasyAbp.EShop.Payments/src/EasyAbp.EShop.Payments.Application/EasyAbp/EShop/Payments/Payments/PaymentAppService.cs)
        </details>

    3. Handle the payment created distributed event to get and remember the `PaymentId`.
        <details>
        <summary>See sample code</summary>

        ```csharp
        public class MyCustomPaymentCreatedEventHandler : IDistributedEventHandler<EntityCreatedEto<PaymentEto>>, ITransientDependency
        {
            [UnitOfWork(isTransactional: true)]
            public virtual async Task HandleEventAsync(EntityCreatedEto<PaymentEto> eventData)
            {
                foreach (var item in eventData.Entity.PaymentItems.Where(item => item.ItemType == "MyCustomKeyword"))
                {
                    // Persistence the PaymentId of the ongoing payment, so user can get it in some way.
                }
            }
        }
        ```
        > please refer to the [usage in EShop](https://github.com/EasyAbp/EShop/blob/dev/modules/EasyAbp.EShop.Orders/src/EasyAbp.EShop.Orders.Domain/EasyAbp/EShop/Orders/Orders/OrderPaymentCreatedEventHandler.cs)
        </details>

    4. Handle the payment canceled distributed event to clear the remembered the `PaymentId`.
        <details>
        <summary>See sample code</summary>

        ```csharp
        public class MyCustomPaymentCanceledEventHandler : IDistributedEventHandler<PaymentCanceledEto>, ITransientDependency
        {
            [UnitOfWork(isTransactional: true)]
            public virtual async Task HandleEventAsync(PaymentCanceledEto payment)
            {
                foreach (var item in payment.PaymentItems.Where(item => item.ItemType == "MyCustomKeyword"))
                {
                    // Remove the remembered PaymentId.
                }
            }
        }
        ```
        > please refer to the [usage in EShop](https://github.com/EasyAbp/EShop/blob/dev/modules/EasyAbp.EShop.Orders/src/EasyAbp.EShop.Orders.Domain/EasyAbp/EShop/Orders/Orders/OrderPaymentCanceledEventHandler.cs)
        </details>

    5. Handle the payment completed distributed event:
        <details>
        <summary>See sample code</summary>

        ```csharp
        public class MyCustomPaymentCompletedEventHandler : IDistributedEventHandler<PaymentCompletedEto>, ITransientDependency
        {
            [UnitOfWork(isTransactional: true)]
            public virtual async Task HandleEventAsync(PaymentCompletedEto payment)
            {
                foreach (var item in payment.PaymentItems.Where(item => item.ItemType == "MyCustomKeyword"))
                {
                    // Maybe you can automatically send out the goods to the customer here.
                }
            }
        }
        ```
        > please refer to the [usage in EShop](https://github.com/EasyAbp/EShop/blob/dev/modules/EasyAbp.EShop.Orders/src/EasyAbp.EShop.Orders.Domain/EasyAbp/EShop/Orders/Orders/OrderPaymentCompletedEventHandler.cs)
        </details>
    </details>

## Roadmap

- [ ] Unit tests.

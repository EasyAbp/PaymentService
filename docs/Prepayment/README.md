# PaymentService.Prepayment

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2FEasyAbp%2FPaymentService%2Fmaster%2FDirectory.Build.props)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/EasyAbp.PaymentService.Prepayment.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.Prepayment.Domain.Shared)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.PaymentService.Prepayment.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.Prepayment.Domain.Shared)
[![Discord online](https://badgen.net/discord/online-members/S6QaezrCRq?label=Discord)](https://discord.gg/S6QaezrCRq)
[![GitHub stars](https://img.shields.io/github/stars/EasyAbp/PaymentService?style=social)](https://www.github.com/EasyAbp/PaymentService)

Prepayment implementation of the EasyAbp.PaymentService module.

## Online Demo

We have launched an online demo for this module: [https://pay.samples.easyabp.io](https://pay.samples.easyabp.io)

## Installation

> Should also install the [PaymentService module](/docs/README.md#getting-started) since this module depends on it.

1. Install the following NuGet packages. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/How-To.md#add-nuget-packages))

    * EasyAbp.PaymentService.Prepayment.Application
    * EasyAbp.PaymentService.Prepayment.Application.Contracts
    * EasyAbp.PaymentService.Prepayment.Domain
    * EasyAbp.PaymentService.Prepayment.Domain.Shared
    * EasyAbp.PaymentService.Prepayment.EntityFrameworkCore
    * EasyAbp.PaymentService.Prepayment.HttpApi
    * EasyAbp.PaymentService.Prepayment.HttpApi.Client
    * (Optional) EasyAbp.PaymentService.Prepayment.MongoDB
    * (Optional) EasyAbp.PaymentService.Prepayment.Web

1. Add `DependsOn(typeof(PaymentServicePrepaymentXxxModule))` attribute to configure the module dependencies. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/How-To.md#add-module-dependencies))

1. Add `builder.ConfigurePaymentServicePrepayment();` to the `OnModelCreating()` method in **MyProjectMigrationsDbContext.cs**.

1. Add EF Core migrations and update your database. See: [ABP document](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC#add-new-migration-update-the-database).

## Usage

1. Register the Prepayment payment method:
    ```csharp
    Configure<PaymentServiceOptions>(options =>
    {
        options.Providers.Configure<PrepaymentPaymentServiceProvider>(PrepaymentPaymentServiceProvider.PaymentMethod);
    });
    ```
    
2. Configure the prepayment to define a account group:
    ```csharp
    Configure<PaymentServicePrepaymentOptions>(options =>
    {
        options.AccountGroups.Configure<DefaultAccountGroup>(accountGroup =>
        {
            accountGroup.Currency = "CNY";
        });
    });
    ```
    > please refer to the `ConfigurePaymentServicePrepayment` method in the [Web module](https://github.com/EasyAbp/PaymentService/blob/master/samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Web/PaymentServiceSampleWebModule.cs) of the sample app.

3. Access the API `/api/paymentService/prepayment/account` (with the request param `UserId`), then the account will be created automatically.

4. Top up your account:

    1. Use the API `/api/paymentService/prepayment/account/topUp` to start top-up.
    
    2. Use the API `/api/paymentService/prepayment/account/{id}` to get the `ExtraProperties.PendingTopUpPaymentId`.
    
    3. Use the API `/api/paymentService/payment/{id}/pay` to finish the payment. (for example you can use WeChatPay to top up your prepayment account, please refer to the document of the payment method you want)
    
    4. If you want to cancel an ongoing payment, please use the API `/api/paymentService/payment/{id}/cancel`.

    > Or just change the balance directly with the management pages if you have the account management permission.

5. Pay with prepayment account.
    1. Users can use the API `/api/paymentService/payment/{id}/pay` to finish the payment, please put the necessary params in the `ExtraProperties`:
    
        ```
        {
            "extraProperties": {
                "AccountId": "82D49C17-9282-4822-9EE9-A0685529D707" // Id of the prepayment account that you use to pay
            }
        }
        ```

    > Skip the following steps if you are using the [EasyAbp.EShop](https://github.com/EasyAbp/EShop).

    <details>
    <summary>See more steps</summary>

    2. Create a payment with the payment method `Prepayment`.
        > Other modules or apps that depend on PaymentService module should create payments via distributed events.

        <details>
        <summary>See sample code</summary>

        ```csharp
        await _distributedEventBus.PublishAsync(new CreatePaymentEto
        {
            TenantId = CurrentTenant.Id,
            UserId = CurrentUser.GetId(),
            PaymentMethod = "Prepayment",   // Should specify the payment method as "Prepayment"
            Currency = "CNY",   // Should be same as the currency configuration of your prepayment account group
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

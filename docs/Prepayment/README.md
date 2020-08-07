# PaymentService.Prepayment

[![NuGet](https://img.shields.io/nuget/v/EasyAbp.PaymentService.Prepayment.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.Prepayment.Domain.Shared)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.PaymentService.Prepayment.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.Prepayment.Domain.Shared)

Prepayment implementation of the EasyAbp.PaymentService module.

## Getting Started

> Should also install the [PaymentService module](../../README.md#getting-started) since this module depends on it.

* Install with [AbpHelper](https://github.com/EasyAbp/AbpHelper.GUI)

    Coming soon.

1. Install the following NuGet packages. (see how)

    * EasyAbp.PaymentService.Prepayment.Application
    * EasyAbp.PaymentService.Prepayment.Application.Contracts
    * EasyAbp.PaymentService.Prepayment.Domain
    * EasyAbp.PaymentService.Prepayment.Domain.Shared
    * EasyAbp.PaymentService.Prepayment.EntityFrameworkCore
    * EasyAbp.PaymentService.Prepayment.HttpApi
    * EasyAbp.PaymentService.Prepayment.HttpApi.Client
    * (Optional) EasyAbp.PaymentService.Prepayment.MongoDB
    * (Optional) EasyAbp.PaymentService.Prepayment.Web

1. Add `DependsOn(typeof(xxx))` attribute to configure the module dependencies. (see how)

1. Add `builder.ConfigurePaymentServicePrepayment();` to the `OnModelCreating()` method in **MyProjectMigrationsDbContext.cs**.

1. Add EF Core migrations and update your database. See: [ABP document](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC#add-new-migration-update-the-database).

## Usage

1. Register the Prepayment payment method:
    ```csharp
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var resolver = context.ServiceProvider.GetRequiredService<IPaymentServiceResolver>();

        resolver.TryRegisterProvider(PrepaymentPaymentServiceProvider.PaymentMethod, typeof(PrepaymentPaymentServiceProvider));
    }
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
    > please refer to the `ConfigurePaymentServicePrepayment` method in the [Web module](../../samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Web/PaymentServiceSampleWebModule.cs) of the sample app.

3. Access the API `/api/paymentService/prepayment/account` (with the request param `UserId`), then the account will be created automatically.

4. Recharge your account:

    1. Use the API `/api/paymentService/prepayment/account/recharge` to start recharging.
    
    2. Use the API `/api/paymentService/prepayment/account/{id}` to get the `ExtraProperties.PendingRechargePaymentId`.
    
    3. Use the API `/api/paymentService/payment/{id}/pay` to finish the payment. (for example you can use WeChatPay to recharge your prepayment account, please refer to the document of the payment method you want)
    
    4. If you want to cancel an ongoing payment, please use the API `/api/paymentService/payment/{id}/cancel`.

> Or just change the balance directly with the management pages if you have the account management permission.

5. Create a payment and handle the payment completed event: **(skip this part if you are using [EasyAbp.EShop](https://github.com/EasyAbp/EShop))**

    1. Create a payment with the payment method `Prepayment` (other modules or apps that depend on PaymentService module should create payments via distributed events):
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

6. Users can use the API `/api/paymentService/payment/{id}/pay` to finish the payment, please put the necessary params in the `ExtraProperties`:
```
{
    "extraProperties": {
        "AccountId": "82D49C17-9282-4822-9EE9-A0685529D707" // Id of the prepayment account that you use to pay
    }
}
```

## Roadmap

- [ ] Unit tests.

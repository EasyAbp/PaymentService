# PaymentService

[![NuGet](https://img.shields.io/nuget/v/EasyAbp.PaymentService.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.Domain.Shared)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.PaymentService.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.Domain.Shared)

An abp application module that provides payment service.

## Getting Started

* Install with [AbpHelper](https://github.com/EasyAbp/AbpHelper.GUI)
    Coming soon.

1. Install the following NuGet packages. (see how)
    * EasyAbp.PaymentService.Application
    * EasyAbp.PaymentService.Application.Contracts
    * EasyAbp.PaymentService.Domain
    * EasyAbp.PaymentService.Domain.Shared
    * EasyAbp.PaymentService.EntityFrameworkCore
    * EasyAbp.PaymentService.HttpApi
    * EasyAbp.PaymentService.HttpApi.Client
    * (Optional) EasyAbp.PaymentService.MongoDB
    * (Optional) EasyAbp.PaymentService.Web

1. Add `DependsOn(typeof(xxx))` attribute to configure the module dependencies. (see how)

1. Add `builder.ConfigurePaymentService();` to the `OnModelCreating()` method in **MyProjectMigrationsDbContext.cs**.

1. Add EF Core migrations and update your database. See: [ABP document](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC#add-new-migration-update-the-database).

## Usage

1. Register the Free payment method, it is used to pay with 0.00 amount:

    ```csharp
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var resolver = context.ServiceProvider.GetRequiredService<IPaymentServiceResolver>();
    
        resolver.TryRegisterProvider(FreePaymentServiceProvider.PaymentMethod, typeof(FreePaymentServiceProvider));
    }
    ```

2. Choose the payment service providers you want:
    * Free
    * [Prepayment](/docs/Prepayment/README.md)
    * [WeChatPay](/docs/WeChatPay/README.md)

![Payment](/docs/images/Payment.png)

## Roadmap

- [x] Prepayment.
- [x] Support WeChatPay.
- [ ] Support Paypal.
- [ ] Support Alipay.
- [ ] Support Bitcoin payment.
- [ ] Unit tests.

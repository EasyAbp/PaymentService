# PaymentService

[![NuGet](https://img.shields.io/nuget/v/EasyAbp.PaymentService.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.Domain.Shared)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.PaymentService.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.Domain.Shared)
[![GitHub stars](https://img.shields.io/github/stars/EasyAbp/PaymentService?style=social)](https://www.github.com/EasyAbp/PaymentService)

An abp application module that provides payment service.

## Online Demo

We have launched an online demo for this module: [https://pay.samples.easyabp.io](https://pay.samples.easyabp.io)

## Installation

1. Install the following NuGet packages. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/How-To.md#add-nuget-packages))

    * EasyAbp.PaymentService.Application
    * EasyAbp.PaymentService.Application.Contracts
    * EasyAbp.PaymentService.Domain
    * EasyAbp.PaymentService.Domain.Shared
    * EasyAbp.PaymentService.EntityFrameworkCore
    * EasyAbp.PaymentService.HttpApi
    * EasyAbp.PaymentService.HttpApi.Client
    * (Optional) EasyAbp.PaymentService.MongoDB
    * (Optional) EasyAbp.PaymentService.Web

1. Add `DependsOn(typeof(PaymentServiceXxxModule))` attribute to configure the module dependencies. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/How-To.md#add-module-dependencies))

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

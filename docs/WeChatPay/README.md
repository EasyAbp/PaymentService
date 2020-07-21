# PaymentService.WeChatPay

[![NuGet](https://img.shields.io/nuget/v/EasyAbp.PaymentService.WeChatPay.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.WeChatPay.Domain.Shared)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.PaymentService.WeChatPay.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.PaymentService.WeChatPay.Domain.Shared)

WeChatPay provider for the EasyAbp.PaymentService module.

## Getting Started

* Install with [AbpHelper](https://github.com/EasyAbp/AbpHelper.GUI)

    Coming soon.

* Install Manually

    1. Install `EasyAbp.PaymentService.WeChatPay.Application` NuGet package to `MyProject.Application` project and add `[DependsOn(PaymentServiceWeChatPayApplicationModule)]` attribute to the module.

    1. Install `EasyAbp.PaymentService.WeChatPay.Application.Contracts` NuGet package to `MyProject.Application.Contracts` project and add `[DependsOn(PaymentServiceWeChatPayApplicationContractsModule)]` attribute to the module.

    1. Install `EasyAbp.PaymentService.WeChatPay.Domain` NuGet package to `MyProject.Domain` project and add `[DependsOn(PaymentServiceWeChatPayDomainModule)]` attribute to the module.

    1. Install `EasyAbp.PaymentService.WeChatPay.Domain.Shared` NuGet package to `MyProject.Domain.Shared` project and add `[DependsOn(PaymentServiceWeChatPayDomainSharedModule)]` attribute to the module.

    1. Install `EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore` NuGet package to `MyProject.EntityFrameworkCore` project and add `[DependsOn(PaymentServiceWeChatPayEntityFrameworkCoreModule)]` attribute to the module.

    1. Install `EasyAbp.PaymentService.WeChatPay.HttpApi` NuGet package to `MyProject.HttpApi` project and add `[DependsOn(PaymentServiceWeChatPayHttpApiModule)]` attribute to the module.

    1. Install `EasyAbp.PaymentService.WeChatPay.HttpApi.Client` NuGet package to `MyProject.HttpApi.Client` project and add `[DependsOn(PaymentServiceWeChatPayHttpApiClientModule)]` attribute to the module.

    1. Install `EasyAbp.PaymentService.WeChatPay.MongoDB` NuGet package to `MyProject.MongoDB` project and add `[DependsOn(PaymentServiceWeChatPayMongoDbModule)]` attribute to the module.

    1. (Optional) If you need MVC UI, install `EasyAbp.PaymentService.WeChatPay.Web` NuGet package to `MyProject.Web` project and add `[DependsOn(PaymentServiceWeChatPayWebModule)]` attribute to the module.

    1. Add `builder.ConfigurePaymentService();` to OnModelCreating method in `MyProjectMigrationsDbContext.cs`.

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

## Roadmap

- [ ] More payment providers.
- [ ] Unit tests.

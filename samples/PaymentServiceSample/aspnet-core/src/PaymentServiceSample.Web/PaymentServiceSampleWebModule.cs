using System;
using System.IO;
using EasyAbp.PaymentService;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Prepayment;
using EasyAbp.PaymentService.Prepayment.Options;
using EasyAbp.PaymentService.Prepayment.PaymentService;
using EasyAbp.PaymentService.Prepayment.Web;
using EasyAbp.PaymentService.Prepayment.WithdrawalRequests;
using EasyAbp.PaymentService.Web;
using EasyAbp.PaymentService.WeChatPay;
using EasyAbp.PaymentService.WeChatPay.Web;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentServiceSample.EntityFrameworkCore;
using PaymentServiceSample.Localization;
using PaymentServiceSample.MultiTenancy;
using PaymentServiceSample.Web.Menus;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace PaymentServiceSample.Web
{
    [DependsOn(
        typeof(PaymentServiceSampleHttpApiModule),
        typeof(PaymentServiceSampleApplicationModule),
        typeof(PaymentServiceSampleEntityFrameworkCoreDbMigrationsModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpTenantManagementWebModule),
        typeof(AbpFeatureManagementWebModule),
        typeof(AbpSettingManagementWebModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(PaymentServiceWebModule),
        typeof(PaymentServicePrepaymentWebModule),
        typeof(PaymentServiceWeChatPayWebModule)
        )]
    public class PaymentServiceSampleWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(PaymentServiceSampleResource),
                    typeof(PaymentServiceSampleDomainModule).Assembly,
                    typeof(PaymentServiceSampleDomainSharedModule).Assembly,
                    typeof(PaymentServiceSampleApplicationModule).Assembly,
                    typeof(PaymentServiceSampleApplicationContractsModule).Assembly,
                    typeof(PaymentServiceSampleWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureUrls(configuration);
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureNavigationServices();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);
            ConfigurePaymentServicePrepayment();
        }

        private void ConfigurePaymentServicePrepayment()
        {
            Configure<PaymentServicePrepaymentOptions>(options =>
            {
                options.AccountGroups.Configure<DefaultAccountGroup>(accountGroup =>
                {
                    accountGroup.Currency = "CNY";
                });
                
                options.AccountGroups.Configure<CustomAccountGroup>(accountGroup =>
                {
                    accountGroup.Currency = "CNY";
                    accountGroup.AllowedUsingToTopUpOtherAccounts = true;
                });

                options.WithdrawalMethods.Configure<NullWithdrawalMethod>(withdrawalMethod =>
                {
                    withdrawalMethod.AccountWithdrawalProviderType = typeof(NullAccountWithdrawalProvider);
                    withdrawalMethod.DailyMaximumWithdrawalAmountEachAccount = 1m;
                });
                
                options.WithdrawalMethods.Configure<ManualWithdrawalMethod>(withdrawalMethod =>
                {
                    withdrawalMethod.AccountWithdrawalProviderType = typeof(ManualAccountWithdrawalProvider);
                    withdrawalMethod.DailyMaximumWithdrawalAmountEachAccount = 5m;
                });
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "PaymentServiceSample";
                });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<PaymentServiceSampleWebModule>();

            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceSampleDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PaymentServiceSample.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceSampleDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PaymentServiceSample.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceSampleApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PaymentServiceSample.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceSampleApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}PaymentServiceSample.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceSampleWebModule>(hostingEnvironment.ContentRootPath);
                    
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Web"));
                    
                    
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceWeChatPayDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService.WeChatPay{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.WeChatPay.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceWeChatPayDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService.WeChatPay{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.WeChatPay.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceWeChatPayApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService.WeChatPay{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.WeChatPay.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceWeChatPayApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService.WeChatPay{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.WeChatPay.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServiceWeChatPayWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService.WeChatPay{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.WeChatPay.Web"));
                    
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServicePrepaymentDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Prepayment{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Prepayment.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServicePrepaymentDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Prepayment{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Prepayment.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServicePrepaymentApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Prepayment{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Prepayment.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServicePrepaymentApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Prepayment{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Prepayment.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<PaymentServicePrepaymentWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}modules{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Prepayment{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}EasyAbp.PaymentService.Prepayment.Web"));
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PaymentServiceSampleResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );

                options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            });
        }

        private void ConfigureNavigationServices()
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new PaymentServiceSampleMenuContributor());
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(PaymentServiceSampleApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentServiceSample API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
            
            // // Register the Swagger services
            // services.AddSwaggerDocument(options =>
            // {
            //     options.DocumentName = "EasyAbpPaymentService";
            //     options.ApiGroupNames = new[] {"EasyAbpPaymentService"};
            //     options.Title = "EasyAbp PaymentService API";
            // });
            //
            // services.AddSwaggerDocument(options =>
            // {
            //     options.DocumentName = "EasyAbpPaymentServicePrepayment";
            //     options.ApiGroupNames = new[] {"EasyAbpPaymentServicePrepayment"};
            //     options.Title = "EasyAbp PaymentService Prepayment API";
            // });
            //
            // services.AddSwaggerDocument(options =>
            // {
            //     options.DocumentName = "EasyAbpPaymentServiceWeChatPay";
            //     options.ApiGroupNames = new[] {"EasyAbpPaymentServiceWeChatPay"};
            //     options.Title = "EasyAbp PaymentService WeChatPay API";
            // });
            //
            // services.AddSwaggerDocument(options =>
            // {
            //     options.Title = "PaymentServiceSample API";
            // });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentServiceSample API");
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();

            RegisterPaymentMethods(context);

            using (var scope = context.ServiceProvider.CreateScope())
            {
                AsyncHelper.RunSync(async () =>
                {
                    await scope.ServiceProvider
                        .GetRequiredService<IDataSeeder>()
                        .SeedAsync();
                });
            }
        }

        private static void RegisterPaymentMethods(IServiceProviderAccessor context)
        {
            var resolver = context.ServiceProvider.GetService<IPaymentServiceResolver>();

            resolver.TryRegisterProvider(FreePaymentServiceProvider.PaymentMethod, typeof(FreePaymentServiceProvider));
            resolver.TryRegisterProvider(PrepaymentPaymentServiceProvider.PaymentMethod, typeof(PrepaymentPaymentServiceProvider));
            resolver.TryRegisterProvider(WeChatPayPaymentServiceProvider.PaymentMethod, typeof(WeChatPayPaymentServiceProvider));
        }
    }
}

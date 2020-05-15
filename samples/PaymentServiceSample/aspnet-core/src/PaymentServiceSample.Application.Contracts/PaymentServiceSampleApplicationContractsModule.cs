﻿using EasyAbp.PaymentService;
using EasyAbp.PaymentService.WeChatPay;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace PaymentServiceSample
{
    [DependsOn(
        typeof(PaymentServiceSampleDomainSharedModule),
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpIdentityApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpTenantManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule),
        typeof(PaymentServiceApplicationContractsModule),
        typeof(PaymentServiceWeChatPayApplicationContractsModule)
    )]
    public class PaymentServiceSampleApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PaymentServiceSampleDtoExtensions.Configure();
        }
    }
}

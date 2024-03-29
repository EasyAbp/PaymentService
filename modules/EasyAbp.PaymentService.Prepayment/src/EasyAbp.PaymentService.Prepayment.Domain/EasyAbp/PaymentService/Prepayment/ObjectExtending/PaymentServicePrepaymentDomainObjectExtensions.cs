﻿using System;
using EasyAbp.PaymentService.Payments;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace EasyAbp.PaymentService.Prepayment.ObjectExtending
{
    public static class PaymentServicePrepaymentDomainObjectExtensions
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                /* You can configure extension properties to entities or other object types
                 * defined in the depended modules.
                 * 
                 * If you are using EF Core and want to map the entity extension properties to new
                 * table fields in the database, then configure them in the PaymentServiceSampleEfCoreEntityExtensionMappings
                 *
                 * Example:
                 *
                 * ObjectExtensionManager.Instance
                 *    .AddOrUpdateProperty<IdentityRole, string>("Title");
                 *
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Object-Extensions
                 */

                ObjectExtensionManager.Instance
                    .AddOrUpdate(new[]
                        {
                            typeof(Payment)
                        },
                        config =>
                        {
                            config.AddOrUpdateProperty<Guid?>(PrepaymentConsts.PaymentAccountIdPropertyName);
                        });
            });
        }
    }
}
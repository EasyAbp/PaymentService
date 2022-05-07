using System;
using EasyAbp.PaymentService.Payments.Dtos;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace EasyAbp.PaymentService.WeChatPay.ObjectExtending
{
    public static class PaymentServiceWeChatPayApplicationObjectExtensions
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
                            typeof(PaymentDto)
                        },
                        config =>
                        {
                            config.AddOrUpdateProperty<string>("appid");
                            config.AddOrUpdateProperty<string>("trade_type");
                            config.AddOrUpdateProperty<string>("prepay_id");
                            config.AddOrUpdateProperty<string>("code_url");
                        });
            });
        }
    }
}
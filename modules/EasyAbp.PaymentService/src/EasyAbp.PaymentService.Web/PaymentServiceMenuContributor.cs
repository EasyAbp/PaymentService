using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using EasyAbp.PaymentService.Localization;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.PaymentService.Web
{
    public class PaymentServiceMenuContributor : IMenuContributor
    {
        public virtual async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenu(context);
            }
        }

        private async Task ConfigureMainMenu(MenuConfigurationContext context)
        {
            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<PaymentServiceResource>>();            //Add main menu items.

            var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();

            var paymentManagementMenuItem = new ApplicationMenuItem("PaymentManagement", l["Menu:PaymentManagement"]);

            if (await authorizationService.IsGrantedAsync(PaymentServicePermissions.PaymentService.Manage))
            {
                paymentManagementMenuItem.AddItem(
                    new ApplicationMenuItem("Payment", l["Menu:Payment"], "/PaymentService/Payments/Payment")
                );
            }
            
            if (await authorizationService.IsGrantedAsync(PaymentServicePermissions.Refunds.Manage))
            {
                paymentManagementMenuItem.AddItem(
                    new ApplicationMenuItem("Refund", l["Menu:Refund"], "/PaymentService/Refunds/Refund")
                );
            }
            
            if (!paymentManagementMenuItem.Items.IsNullOrEmpty())
            {
                var paymentServiceMenuItem = context.Menu.Items.GetOrAdd(i => i.Name == "EasyAbpPaymentService",
                    () => new ApplicationMenuItem("EasyAbpPaymentService", l["Menu:EasyAbpPaymentService"]));
                
                paymentServiceMenuItem.Items.Add(paymentManagementMenuItem);
            }
        }
    }
}

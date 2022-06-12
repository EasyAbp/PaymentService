using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using EasyAbp.PaymentService.Localization;
using EasyAbp.PaymentService.Web.Menus;
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
            var l = context.GetLocalizer<PaymentServiceResource>();            //Add main menu items.

            var paymentManagementMenuItem = new ApplicationMenuItem(PaymentServiceMenus.Prefix, l["Menu:PaymentService"], icon: "fa fa-credit-card");

            if (await context.IsGrantedAsync(PaymentServicePermissions.Payments.Manage))
            {
                paymentManagementMenuItem.AddItem(
                    new ApplicationMenuItem(PaymentServiceMenus.Payment, l["Menu:Payment"], "/PaymentService/Payments/Payment")
                );
            }
            
            if (await context.IsGrantedAsync(PaymentServicePermissions.Refunds.Manage))
            {
                paymentManagementMenuItem.AddItem(
                    new ApplicationMenuItem(PaymentServiceMenus.Refund, l["Menu:Refund"], "/PaymentService/Refunds/Refund")
                );
            }
            
            if (!paymentManagementMenuItem.Items.IsNullOrEmpty())
            {
                context.Menu.Items.GetOrAdd(i => i.Name == PaymentServiceMenus.Prefix,
                    () => paymentManagementMenuItem);
            }
        }
    }
}

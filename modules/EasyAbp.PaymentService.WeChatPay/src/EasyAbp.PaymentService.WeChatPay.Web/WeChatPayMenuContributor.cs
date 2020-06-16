using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.WeChatPay.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using EasyAbp.PaymentService.WeChatPay.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using EasyAbp.PaymentService.WeChatPay.Localization;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.PaymentService.WeChatPay.Web
{
    public class WeChatPayMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenu(context);
            }
        }

        private async Task ConfigureMainMenu(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<WeChatPayResource>();            //Add main menu items.

            var weChatPayManagementMenuItem = new ApplicationMenuItem("EasyAbpPaymentServiceWeChatPay", l["Menu:WeChatPayManagement"]);

            if (await context.IsGrantedAsync(WeChatPayPermissions.PaymentRecords.Default))
            {
                weChatPayManagementMenuItem.AddItem(
                    new ApplicationMenuItem("EasyAbpPaymentServiceWeChatPayPaymentRecord", l["Menu:PaymentRecord"], "/PaymentService/WeChatPay/PaymentRecords/PaymentRecord")
                );
            }
            
            if (await context.IsGrantedAsync(WeChatPayPermissions.RefundRecords.Default))
            {
                weChatPayManagementMenuItem.AddItem(
                    new ApplicationMenuItem("EasyAbpPaymentServiceWeChatPayRefundRecord", l["Menu:RefundRecord"], "/PaymentService/WeChatPay/RefundRecords/RefundRecord")
                );
            }

            if (!weChatPayManagementMenuItem.Items.IsNullOrEmpty())
            {
                var paymentServiceMenuItem = context.Menu.Items.GetOrAdd(i => i.Name == "EasyAbpPaymentService",
                    () => new ApplicationMenuItem("EasyAbpPaymentService", l["Menu:EasyAbpPaymentService"]));
                
                paymentServiceMenuItem.Items.Add(weChatPayManagementMenuItem);
            }
        }
    }
}

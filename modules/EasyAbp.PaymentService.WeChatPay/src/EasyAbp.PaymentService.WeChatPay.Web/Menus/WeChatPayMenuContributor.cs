using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.WeChatPay.Authorization;
using EasyAbp.PaymentService.WeChatPay.Localization;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.PaymentService.WeChatPay.Web.Menus
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

            var weChatPayManagementMenuItem = new ApplicationMenuItem(WeChatPayMenus.Prefix, l["Menu:WeChatPayManagement"]);

            if (await context.IsGrantedAsync(WeChatPayPermissions.PaymentRecords.Default))
            {
                weChatPayManagementMenuItem.AddItem(
                    new ApplicationMenuItem(WeChatPayMenus.PaymentRecord, l["Menu:PaymentRecord"], "/PaymentService/WeChatPay/PaymentRecords/PaymentRecord")
                );
            }
            
            if (await context.IsGrantedAsync(WeChatPayPermissions.RefundRecords.Default))
            {
                weChatPayManagementMenuItem.AddItem(
                    new ApplicationMenuItem(WeChatPayMenus.RefundRecord, l["Menu:RefundRecord"], "/PaymentService/WeChatPay/RefundRecords/RefundRecord")
                );
            }

            if (!weChatPayManagementMenuItem.Items.IsNullOrEmpty())
            {
                var paymentServiceMenuItem = context.Menu.GetAdministration().Items.GetOrAdd(i => i.Name == WeChatPayMenus.ModuleGroupPrefix,
                    () => new ApplicationMenuItem(WeChatPayMenus.ModuleGroupPrefix, l["Menu:PaymentService"], icon: "fa fa-credit-card"));
                
                paymentServiceMenuItem.Items.Add(weChatPayManagementMenuItem);
            }
        }
    }
}

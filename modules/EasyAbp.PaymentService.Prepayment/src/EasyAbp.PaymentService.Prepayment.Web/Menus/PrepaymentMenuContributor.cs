using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Localization;
using EasyAbp.PaymentService.Prepayment.Permissions;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.PaymentService.Prepayment.Web.Menus
{
    public class PrepaymentMenuContributor : IMenuContributor
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
            var l = context.GetLocalizer<PrepaymentResource>();
             //Add main menu items.

             var prepaymentManagementMenuItem = new ApplicationMenuItem(PrepaymentMenus.Prefix, l["Menu:PrepaymentManagement"]);

            if (await context.IsGrantedAsync(PrepaymentPermissions.Account.Default))
            {
                prepaymentManagementMenuItem.AddItem(
                    new ApplicationMenuItem(PrepaymentMenus.Account, l["Menu:Account"], "/PaymentService/Prepayment/Accounts/Account")
                );
            }
            
            if (!prepaymentManagementMenuItem.Items.IsNullOrEmpty())
            {
                var paymentServiceMenuItem = context.Menu.Items.GetOrAdd(i => i.Name == PrepaymentMenus.ModuleGroupPrefix,
                    () => new ApplicationMenuItem(PrepaymentMenus.ModuleGroupPrefix, l["Menu:EasyAbpPaymentService"]));
                
                paymentServiceMenuItem.Items.Add(prepaymentManagementMenuItem);
            }
        }
    }
}

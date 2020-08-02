using System.Threading.Tasks;
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

        private Task ConfigureMainMenu(MenuConfigurationContext context)
        {
            //Add main menu items.

            return Task.CompletedTask;
        }
    }
}
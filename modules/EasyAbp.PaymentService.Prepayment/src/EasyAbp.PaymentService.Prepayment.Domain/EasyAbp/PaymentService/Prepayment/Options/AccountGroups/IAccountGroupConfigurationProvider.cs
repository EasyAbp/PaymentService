namespace EasyAbp.PaymentService.Prepayment.Options.AccountGroups
{
    public interface IAccountGroupConfigurationProvider
    {
        AccountGroupConfiguration Get(string accountGroupName);
    }
}
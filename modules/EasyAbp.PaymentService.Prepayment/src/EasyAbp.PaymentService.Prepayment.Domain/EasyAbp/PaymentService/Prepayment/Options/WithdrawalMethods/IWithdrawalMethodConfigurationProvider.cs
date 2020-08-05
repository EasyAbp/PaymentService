namespace EasyAbp.PaymentService.Prepayment.Options.WithdrawalMethods
{
    public interface IWithdrawalMethodConfigurationProvider
    {
        WithdrawalMethodConfiguration Get(string withdrawalMethodName);
    }
}
using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class PayeeConfigurationMissingValueException : BusinessException
    {
        public PayeeConfigurationMissingValueException(string paymentMethod, string configurationKey) : base(
            message: $"Payment method ({paymentMethod}) is missing configuration: {configurationKey}.")
        {
        }
    }
}
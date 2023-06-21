using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class AmountOverflowException : BusinessException
    {
        public AmountOverflowException(decimal min, decimal max) : base(
            message: $"The amount should be greater than {min} and less then {max}")
        {
        }

        public AmountOverflowException(string field, decimal min, decimal max) : base(
            message: $"The {field} should be greater than {min} and less then {max}")
        {
        }
    }
}
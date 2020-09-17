using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class TopUpIsAlreadyInProgressException : BusinessException
    {
        public TopUpIsAlreadyInProgressException()
            : base("TopUpIsAlreadyInProgress","Another top up for the account is already in progress.")
        {
        }
    }
}
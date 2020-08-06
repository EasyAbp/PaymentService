using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.PaymentService
{
    public class UserIsNotAccountOwnerException : BusinessException
    {
        public UserIsNotAccountOwnerException(Guid userId, Guid accountId) : base(
            message: $"The user {userId} is not the owner of the account ({accountId}).")
        {
        }
    }
}
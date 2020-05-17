using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class UserOpenIdNotFoundException : BusinessException
    {
        public UserOpenIdNotFoundException(Guid userId) : base(message: $"The OpenId of user {userId} was not found.")
        {
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace EasyAbp.PaymentService.WeChatPay
{
    [Dependency(ServiceLifetime.Singleton, TryRegister = true)]
    public class ClaimPaymentOpenIdProvider : IPaymentOpenIdProvider
    {
        public const string OpenIdClaimType = "WeChatOpenId";

        private readonly ICurrentUser _currentUser;

        public ClaimPaymentOpenIdProvider(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        
        public Task<string> FindUserOpenIdAsync(string appId, Guid userId)
        {
            return Task.FromResult(userId == _currentUser.Id
                ? _currentUser.FindClaim(OpenIdClaimType)?.Value ?? throw new UserOpenIdNotFoundException(userId)
                : throw new AbpAuthorizationException("Should ensure current user is the order owner."));
        }
    }
}
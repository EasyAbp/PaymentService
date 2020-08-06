using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.PaymentService.Prepayment.PaymentService
{
    public class PrepaymentPayeeAccountProvider : IPrepaymentPayeeAccountProvider, ITransientDependency
    {
        private readonly IAccountRepository _accountRepository;

        public PrepaymentPayeeAccountProvider(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        
        public virtual Task<string> GetPayeeAccountAsync(Account account)
        {
            var payeeAccount = $"{account.AccountGroupName},{account.Id}";
            
            return Task.FromResult(payeeAccount);
        }

        public virtual async Task<Account> GetAccountByPayeeAccountAsync(string payeeAccount)
        {
            var accountId = Guid.Parse(payeeAccount.Split(new[] {','}, 2)[1]);

            return await _accountRepository.GetAsync(accountId);
        }
    }
}
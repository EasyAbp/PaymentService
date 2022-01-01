using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts;
using EasyAbp.PaymentService.Prepayment.Options.AccountGroups;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace EasyAbp.PaymentService.Prepayment
{
    public class PrepaymentDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IGuidGenerator _guidGenerator;

        public PrepaymentDataSeedContributor(
            IAccountRepository accountRepository,
            IGuidGenerator guidGenerator)
        {
            _accountRepository = accountRepository;
            _guidGenerator = guidGenerator;
        }
        
        public async Task SeedAsync(DataSeedContext context)
        {
            /* Instead of returning the Task.CompletedTask, you can insert your test data
             * at this point!
             */

            await _accountRepository.InsertAsync(
                new Account(PrepaymentTestConsts.AccountId, null,
                    AccountGroupNameAttribute.GetAccountGroupName<DefaultAccountGroup>(), PrepaymentTestConsts.UserId,
                    PrepaymentTestConsts.AccountBaseBalance, 0m), true);
        }
    }
}
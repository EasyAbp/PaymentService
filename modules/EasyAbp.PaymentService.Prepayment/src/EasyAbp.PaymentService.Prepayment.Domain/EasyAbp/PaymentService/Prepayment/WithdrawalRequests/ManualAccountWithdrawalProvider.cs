using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests
{
    public class ManualAccountWithdrawalProvider : AccountWithdrawalProvider, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IWithdrawalRequestRepository _withdrawalRequestRepository;

        public ManualAccountWithdrawalProvider(
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IWithdrawalRequestRepository withdrawalRequestRepository,
            IAccountWithdrawalManager accountWithdrawalManager) : base(accountWithdrawalManager)
        {
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _withdrawalRequestRepository = withdrawalRequestRepository;
        }

        public override async Task OnStartWithdrawalAsync(Account account, string withdrawalMethod, decimal amount,
            ExtraPropertyDictionary inputExtraProperties)
        {
            var request = new WithdrawalRequest(_guidGenerator.Create(), _currentTenant.Id, account.Id, account.UserId, amount);

            await _withdrawalRequestRepository.InsertAsync(request, true);
        }

        public override Task OnCompleteWithdrawalAsync(Account account)
        {
            return Task.CompletedTask;
        }

        public override Task OnCancelWithdrawalAsync(Account account)
        {
            return Task.CompletedTask;
        }
    }
}
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests
{
    public static class WithdrawalRequestEfCoreQueryableExtensions
    {
        public static IQueryable<WithdrawalRequest> IncludeDetails(this IQueryable<WithdrawalRequest> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                // .Include(x => x.xxx) // TODO: AbpHelper generated
                ;
        }
    }
}
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRecords
{
    public static class WithdrawalRecordEfCoreQueryableExtensions
    {
        public static IQueryable<WithdrawalRecord> IncludeDetails(this IQueryable<WithdrawalRecord> queryable, bool include = true)
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
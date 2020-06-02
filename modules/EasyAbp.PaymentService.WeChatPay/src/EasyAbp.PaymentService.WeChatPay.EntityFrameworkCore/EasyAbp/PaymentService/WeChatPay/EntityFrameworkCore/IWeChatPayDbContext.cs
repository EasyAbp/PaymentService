using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;

namespace EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore
{
    [ConnectionStringName(WeChatPayDbProperties.ConnectionStringName)]
    public interface IWeChatPayDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
        DbSet<RefundRecord> RefundRecords { get; set; }
        DbSet<PaymentRecord> PaymentRecords { get; set; }
    }
}

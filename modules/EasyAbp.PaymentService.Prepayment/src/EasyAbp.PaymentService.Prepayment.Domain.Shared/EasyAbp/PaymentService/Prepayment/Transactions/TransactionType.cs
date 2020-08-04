using System;

namespace EasyAbp.PaymentService.Prepayment.Transactions
{
    [Flags]
    public enum TransactionType
    {
        Debit = 1,
        Credit = 2
    }
}
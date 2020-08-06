using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EasyAbp.PaymentService.Prepayment.Accounts.Dtos
{
    [Serializable]
    public class ChangeBalanceInput : IValidatableObject
    {
        public decimal ChangedBalance { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ChangedBalance == decimal.Zero)
            {
                yield return new ValidationResult(
                    "The ChangedBalance should not be zero!",
                    new[] { "ChangedBalance" }
                );
            }
        }
    }
}
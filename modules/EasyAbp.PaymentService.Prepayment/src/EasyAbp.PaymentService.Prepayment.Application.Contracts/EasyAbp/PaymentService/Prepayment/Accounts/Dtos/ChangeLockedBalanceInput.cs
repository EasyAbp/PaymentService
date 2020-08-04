using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EasyAbp.PaymentService.Prepayment.Accounts.Dtos
{
    [Serializable]
    public class ChangeLockedBalanceInput : IValidatableObject
    {
        public Guid AccountId { get; set; }
        
        public decimal ChangedLockedBalance { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ChangedLockedBalance == decimal.Zero)
            {
                yield return new ValidationResult(
                    "The ChangedLockedBalance should not be zero!",
                    new[] { "ChangedLockedBalance" }
                );
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Refunds.Dtos
{
    public class RefundItemDto : FullAuditedEntityDto<Guid>, IRefundItem
    {
        public Guid PaymentItemId { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        public string CustomerRemark { get; set; }
        
        public string StaffRemark { get; set; }
    }
}
using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Payments.Dtos;

public class GetPaymentListInput : PagedAndSortedResultRequestDto
{
    public Guid? UserId { get; set; }
}
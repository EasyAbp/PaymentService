using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentAppService :
        IReadOnlyAppService< 
            PaymentDto, 
            Guid, 
            GetPaymentListInput>
    {
        Task<ListResultDto<PaymentMethodDto>> GetListPaymentMethod();
        
        Task<PaymentDto> PayAsync(Guid id, PayInput input);
        
        Task<PaymentDto> CancelAsync(Guid id);

        Task<PaymentDto> RefundRollbackAsync(Guid id);
    }
}
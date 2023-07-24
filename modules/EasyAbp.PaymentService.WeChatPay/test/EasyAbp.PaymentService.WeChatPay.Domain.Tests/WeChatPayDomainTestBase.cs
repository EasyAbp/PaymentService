
/* Inherit from this class for your domain layer tests.
     * See SampleManager_Tests for example.
     */
namespace EasyAbp.PaymentService.WeChatPay;

public abstract class WeChatPayDomainTestBase : WeChatPayTestBase<PaymentServiceWeChatPayDomainTestModule>
{

}
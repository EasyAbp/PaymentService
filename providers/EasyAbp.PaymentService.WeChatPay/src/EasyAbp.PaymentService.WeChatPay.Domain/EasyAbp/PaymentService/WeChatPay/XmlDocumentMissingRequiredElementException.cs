using Volo.Abp;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class XmlDocumentMissingRequiredElementException : BusinessException
    {
        public XmlDocumentMissingRequiredElementException(string elementTag) : base(
            message: $"XmlDocument missing required element: {elementTag}")
        {
        }
    }
}
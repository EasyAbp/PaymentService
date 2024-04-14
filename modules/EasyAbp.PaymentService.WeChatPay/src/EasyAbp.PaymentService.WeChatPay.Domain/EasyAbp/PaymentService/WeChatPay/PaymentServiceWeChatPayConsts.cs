namespace EasyAbp.PaymentService.WeChatPay;

public static class PaymentServiceWeChatPayConsts
{
    /// <summary>
    /// 用于核对：支付是否来源于本模块。
    /// </summary>
    public static string Attach { get; set; } = "EasyAbpPaymentService";

    /// <summary>
    /// 创建支付时的description默认值，显示在用户支付界面上用于描述商品。
    /// </summary>
    public static string DefaultDescriptionOnPaymentCreation { get; set; } = "商品支付";

    /// <summary>
    /// 自动退款时，使用的退款理由。
    /// </summary>
    public static string InvalidPaymentAutoRefundDisplayReason { get; set; } = "接收到无效的支付成功通知，执行自动退款";
}
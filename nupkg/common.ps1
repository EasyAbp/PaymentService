# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

# List of projects
$projects = (

    "src/EasyAbp.PaymentService.Application",
    "src/EasyAbp.PaymentService.Application.Contracts",
    "src/EasyAbp.PaymentService.Domain",
    "src/EasyAbp.PaymentService.Domain.Shared",
    "src/EasyAbp.PaymentService.EntityFrameworkCore",
    "src/EasyAbp.PaymentService.HttpApi",
    "src/EasyAbp.PaymentService.HttpApi.Client",
    "src/EasyAbp.PaymentService.MongoDB",
    "src/EasyAbp.PaymentService.Web",
	
    "providers/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Application",
    "providers/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Application.Contracts",
    "providers/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Domain",
    "providers/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Domain.Shared",
    "providers/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore",
    "providers/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.HttpApi",
    "providers/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.HttpApi.Client",
    "providers/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.MongoDB",
    "providers/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Web"
)
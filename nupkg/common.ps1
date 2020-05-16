# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

# List of projects
$projects = (

    "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Application",
    "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Application.Contracts",
    "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Domain",
    "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Domain.Shared",
    "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.EntityFrameworkCore",
    "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.HttpApi",
    "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.HttpApi.Client",
    "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.MongoDB",
    "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Web",
	
    "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Application",
    "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Application.Contracts",
    "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Domain",
    "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Domain.Shared",
    "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore",
    "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.HttpApi",
    "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.HttpApi.Client",
    "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.MongoDB",
    "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Web"
)
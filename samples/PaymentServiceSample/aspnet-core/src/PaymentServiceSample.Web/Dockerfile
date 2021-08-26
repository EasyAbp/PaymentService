#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Web/PaymentServiceSample.Web.csproj", "samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Web/"]
COPY ["samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.EntityFrameworkCore/PaymentServiceSample.EntityFrameworkCore.csproj", "samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.EntityFrameworkCore/"]
COPY ["modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.EntityFrameworkCore/EasyAbp.PaymentService.EntityFrameworkCore.csproj", "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.EntityFrameworkCore/"]
COPY ["modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Domain/EasyAbp.PaymentService.Domain.csproj", "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Domain/"]
COPY ["modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Domain.Shared/EasyAbp.PaymentService.Domain.Shared.csproj", "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Domain.Shared/"]
COPY ["modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore/EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore.csproj", "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore/"]
COPY ["modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Domain/EasyAbp.PaymentService.WeChatPay.Domain.csproj", "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Domain/"]
COPY ["modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Domain.Shared/EasyAbp.PaymentService.WeChatPay.Domain.Shared.csproj", "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Domain.Shared/"]
COPY ["modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.EntityFrameworkCore/EasyAbp.PaymentService.Prepayment.EntityFrameworkCore.csproj", "modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.EntityFrameworkCore/"]
COPY ["modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.Domain/EasyAbp.PaymentService.Prepayment.Domain.csproj", "modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.Domain/"]
COPY ["modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.Domain.Shared/EasyAbp.PaymentService.Prepayment.Domain.Shared.csproj", "modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.Domain.Shared/"]
COPY ["samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Domain/PaymentServiceSample.Domain.csproj", "samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Domain/"]
COPY ["samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Domain.Shared/PaymentServiceSample.Domain.Shared.csproj", "samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Domain.Shared/"]
COPY ["modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Web/EasyAbp.PaymentService.Web.csproj", "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Web/"]
COPY ["modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.HttpApi/EasyAbp.PaymentService.HttpApi.csproj", "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.HttpApi/"]
COPY ["modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Application.Contracts/EasyAbp.PaymentService.Application.Contracts.csproj", "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Application.Contracts/"]
COPY ["samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Application/PaymentServiceSample.Application.csproj", "samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Application/"]
COPY ["modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Application/EasyAbp.PaymentService.WeChatPay.Application.csproj", "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Application/"]
COPY ["modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Application.Contracts/EasyAbp.PaymentService.WeChatPay.Application.Contracts.csproj", "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Application.Contracts/"]
COPY ["samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Application.Contracts/PaymentServiceSample.Application.Contracts.csproj", "samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Application.Contracts/"]
COPY ["modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.Application.Contracts/EasyAbp.PaymentService.Prepayment.Application.Contracts.csproj", "modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.Application.Contracts/"]
COPY ["modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.Application/EasyAbp.PaymentService.Prepayment.Application.csproj", "modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.Application/"]
COPY ["modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Application/EasyAbp.PaymentService.Application.csproj", "modules/EasyAbp.PaymentService/src/EasyAbp.PaymentService.Application/"]
COPY ["samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.HttpApi/PaymentServiceSample.HttpApi.csproj", "samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.HttpApi/"]
COPY ["modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.HttpApi/EasyAbp.PaymentService.WeChatPay.HttpApi.csproj", "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.HttpApi/"]
COPY ["modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.HttpApi/EasyAbp.PaymentService.Prepayment.HttpApi.csproj", "modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.HttpApi/"]
COPY ["modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Web/EasyAbp.PaymentService.WeChatPay.Web.csproj", "modules/EasyAbp.PaymentService.WeChatPay/src/EasyAbp.PaymentService.WeChatPay.Web/"]
COPY ["modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.Web/EasyAbp.PaymentService.Prepayment.Web.csproj", "modules/EasyAbp.PaymentService.Prepayment/src/EasyAbp.PaymentService.Prepayment.Web/"]
COPY Directory.Build.props .
RUN dotnet restore "samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Web/PaymentServiceSample.Web.csproj"
COPY . .
WORKDIR "/src/samples/PaymentServiceSample/aspnet-core/src/PaymentServiceSample.Web"
RUN dotnet build "PaymentServiceSample.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentServiceSample.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentServiceSample.Web.dll"]

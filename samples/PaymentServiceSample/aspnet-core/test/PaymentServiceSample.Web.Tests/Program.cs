using Microsoft.AspNetCore.Builder;
using PaymentServiceSample;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();

builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("PaymentServiceSample.Web.csproj");
await builder.RunAbpModuleAsync<PaymentServiceSampleWebTestModule>(applicationName: "PaymentServiceSample.Web" );

public partial class Program
{
}
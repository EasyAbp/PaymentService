using Microsoft.AspNetCore.Builder;
using PaymentServiceSample;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<PaymentServiceSampleWebTestModule>();

public partial class Program
{
}
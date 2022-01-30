using System;
using System.Data;
using DataAccess;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderingService.DataAccess;
using OrderingService.Services;
using System.Data.SqlClient;

[assembly: FunctionsStartup(typeof(OrderingService.Startup))]

namespace OrderingService
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<BarClient> (c =>
            {
                c.BaseAddress = new Uri("https://bar-api.azurewebsites.net");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });
            builder.Services.AddScoped<IOrderRepository, InMemoryOrderRepository>();

            builder.Services.AddScoped<SqlConnection>(sp => new SqlConnection(""));
            builder.Services.AddScoped<IDbConnection>(sp => sp.GetRequiredService<SqlConnection>());
        }
    }
}

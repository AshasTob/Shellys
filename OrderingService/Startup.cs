using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderingService.Services;

[assembly: FunctionsStartup(typeof(OrderingService.Startup))]

namespace OrderingService
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<BarClient> (c =>
            {
                c.BaseAddress = new Uri("https://localhost:5001/");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });
            //builder.Services.AddSingleton<ILoggerProvider>();
        }
    }
}

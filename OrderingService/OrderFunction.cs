using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OrderingService.Services;

namespace OrderingService
{
    public class OrderFunction
    {
        private readonly BarClient _bar;
        private readonly ILogger<BarClient> _log;

        public OrderFunction(BarClient client, ILogger<BarClient> log)
        {
            _bar = client;
            _log = log;
        }

        [FunctionName("Order")]
        [OpenApiOperation(operationId: "Order", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Order(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        { 
            _log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name ??= data?.name;

            if (string.IsNullOrEmpty(name))
            {
                return new OkObjectResult("Please input cocktail name");
            }

            var menu = await _bar.GetMenu();

            return new OkObjectResult(menu.ToString());
        }
    }


}


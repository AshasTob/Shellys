using System;
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
using OrderingService.DataAccess;
using OrderingService.Services;

namespace OrderingService
{
    public class OrderFunction
    {
        private readonly BarClient _bar;
        private readonly ILogger<BarClient> _log;
        private readonly IOrderRepository _orders;

        public OrderFunction(BarClient client, ILogger<BarClient> log, IOrderRepository orders)
        {
            _bar = client;
            _log = log;
            _orders = orders;
        }

        [FunctionName("Order")]
        [OpenApiOperation(operationId: "Order", tags: new[] { "id", "name" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The **Id** parameter")]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Order(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        { 
            _log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            if (string.IsNullOrEmpty(name))
            {
                return new OkObjectResult("Please input cocktail name");
            }

            var cocktail = await _bar.GetMenuItem(name);

            int id;
            Order order;
            if (int.TryParse(req.Query["id"], out id))
            {
                order = await _orders.Get(id);
                if (order == null)
                {
                    throw new ArgumentException($"Order with {id} does not exist");
                }
            }
            else
            {
                order = new Order();
            }

            order.TotalPrice += cocktail.Price;
            await _orders.Upsert(order);


            return new OkObjectResult(order);
        }
    }


}


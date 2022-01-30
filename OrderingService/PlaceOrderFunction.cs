using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OrderingService.Services;

namespace OrderingService
{
    public class PlaceOrderFunction
    {
        private readonly BarClient _bar;
        private readonly ILogger<BarClient> _log;
        private readonly IOrderRepository _orders;

        public PlaceOrderFunction(BarClient client, ILogger<BarClient> log, IOrderRepository orders)
        {
            _bar = client;
            _log = log;
            _orders = orders;
        }

        [FunctionName("PlaceOrder")]
        [OpenApiOperation(operationId: "Order", tags: new[] { "id", "name" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The **Id** parameter")]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> PlaceOrder(
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
                    return new BadRequestObjectResult($"Order with id {id} not found");
                }

                if (order.Status == OrderStatus.Finalized)
                {
                    return new BadRequestObjectResult("You cant add cocktails to closed order");
                }
            }
            else
            {
                order = new Order() {Status = OrderStatus.Initiated};
            }

            order.TotalPrice += cocktail.Price;
            order.Id = await _orders.Upsert(order);


            return new OkObjectResult(order);
        }
    }


}


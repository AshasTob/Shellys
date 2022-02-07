using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using OrderTimeOutService.DataAccess;

namespace OrderTimeOutService
{
    public class OrderTimeOutFunction
    {
        private readonly ITimeOutOrderRepository _orders;
        public OrderTimeOutFunction(ITimeOutOrderRepository orders)
        {
            _orders = orders;
        }
        [FunctionName("OrderTimeOut")]
        public async Task RunAsync([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            IEnumerable<Order> timeOutOrders = await _orders.Get(1);
            foreach (Order ord in timeOutOrders)
            {
                await _orders.Update(ord);
            }
        }
    }
}

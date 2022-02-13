using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Repository;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace OrderTimeOutService
{
    public  class OrderTimeOut
    {
        private readonly OrderRepository _orderRepository;

        public OrderTimeOut()
        {
            _orderRepository = new OrderRepository();
        }

        [FunctionName("OrderTimeOut")]
        public async Task Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            List<Order> initiatedOrders = await _orderRepository.GetOrdersByStatus(OrderStatus.Initiated);
            foreach (var order in initiatedOrders)
            {
                order.Status = OrderStatus.Finalized;
                await _orderRepository.Upsert(order);
            }
        }
    }
}

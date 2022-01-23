using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using OrderingService.DataAccess;

namespace OrderingService
{
    public class CompleteOrderFunction
    {
        private readonly IOrderRepository _orders;

        public CompleteOrderFunction(IOrderRepository orders)
        {
            _orders = orders;
        }

        [FunctionName("CompleteOrder")]
        public async Task CompleteOrder([ServiceBusTrigger("complete-order-commands-queue", Connection = "Connection")]CompleteOrderCommand completeCommand, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {completeCommand}");
            var order = await _orders.Get(completeCommand.Id);
            if (order == null)
            {
                throw new ArgumentException($"order with id {completeCommand.Id} not found");
            }

            if (order.Status == OrderStatus.Finalized)
            {
                throw new ArgumentException($"not possible to check out already complete order");
            }
            /**
             * Here some cardpayment methods can be implemented; But we are going just to log a price to console.
             */
            log.LogInformation($"{order.TotalPrice} charged from your card ;)");
            order.Status = OrderStatus.Finalized;
            await _orders.Upsert(order);
        }
    }

    public class CompleteOrderCommand
    {
        public int Id { get; set; }
        public long CreditCardNumber { get; set; }
    }
}

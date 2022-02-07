using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace OrderTimeOutService
{
    public  class OrderTimeOut
    {
        private readonly BarDataBase _barDataBase;

        public OrderTimeOut()
        {
            _barDataBase = new BarDataBase();
        }

        [FunctionName("OrderTimeOut")]
        public void Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            IEnumerable<Order> orders = _barDataBase.Orders.Where(ord => ord.Status == OrderStatus.Initiated);
            foreach (var ord in orders)
            {
                ord.Status = OrderStatus.Finalized;
                _barDataBase.Orders.Update(ord);
            }
            _barDataBase.SaveChanges();
        }
    }
}

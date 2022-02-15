using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Repository;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderTimeOutService.BlobService;
using System.Globalization;

namespace OrderTimeOutService
{
    public  class OrderTimeOut
    {
        private readonly OrderRepository _orderRepository;
        private readonly BlobStorage _blobStorage;
        private readonly IConfigurationBuilder _configuration;

        public OrderTimeOut()
        {
            _orderRepository = new OrderRepository();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _blobStorage = new BlobStorage(_configuration);
        }

        [FunctionName("OrderTimeOut")]
        public async Task Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            List<Order> initiatedOrders = await _orderRepository.GetOrdersByStatus(OrderStatus.Initiated);
            double finalizedOrdersSum = 0;
            foreach (var order in initiatedOrders)
            {
                order.Status = OrderStatus.Finalized;
                finalizedOrdersSum += order.TotalPrice;
                await _orderRepository.Upsert(order);
            }
            string reportData = $"{initiatedOrders.Count},{finalizedOrdersSum.ToString(new CultureInfo("en-us",false))}";
            DateTime dateTime = DateTime.Now;
            string reportFileName = $"{dateTime.Day}_{dateTime.Month}_{dateTime.Year}_report.csv";
            await _blobStorage.UploadContentBlobAsync(reportData, reportFileName);
        }
    }
}

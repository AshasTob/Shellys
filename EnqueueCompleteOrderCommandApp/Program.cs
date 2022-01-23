using System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace EnqueueCompleteOrderCommandApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var connection = "TMP";
            var client = new QueueClient(connection, "complete-order-commands-queue");
            var messageBody = JsonConvert.SerializeObject(new CompleteOrderCommand()
            {
                CreditCardNumber = 4444231385762223,
                Id = 3
            });
            var message = new Message(Encoding.UTF8.GetBytes(messageBody)) { ContentType = "application/json" };

            await client.SendAsync(message);

            Console.WriteLine("Message successfully sent");
        }
    }

    public class CompleteOrderCommand
    {
        public int Id { get; set; }
        public long CreditCardNumber { get; set; }
    }
}


/**
 *          For future ;)
 *
 *          TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
            configuration.InstrumentationKey = "TMP2";
            var telemetryClient = new TelemetryClient(configuration);
            telemetryClient.TrackTrace("Sending message");
            var sw = Stopwatch.StartNew();
 *
 *
 *  
*           sw.Stop();
            telemetryClient.TrackRequest($"Message sent", DateTimeOffset.Now, sw.Elapsed, "200", true);
            telemetryClient.Flush();
            Task.Delay(5000).Wait();
 */

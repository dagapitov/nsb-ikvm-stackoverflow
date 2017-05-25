using NServiceBus;
using System;
using System.Threading.Tasks;

namespace nsb_ikvm_stackoverflow
{
    public class MyMessage : IMessage
    {
        public string Data { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.SelfHosting");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            try
            {
                Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
                var myMessage = new MyMessage();
                await endpointInstance.SendLocal(myMessage)
                    .ConfigureAwait(false);
                Console.ReadKey();
            }
            finally
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
        }
    }
}

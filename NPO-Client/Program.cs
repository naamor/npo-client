using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NPO_Client.Extensions;
using NPO_Client.StreamProcessor;
using System;
using System.Threading.Tasks;

namespace NPO_Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine($"Start NPO-Client");

            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<ISubscriber, MqttController>();
                    services.AddSingleton<IPublisher, KafkaController>();
                    services.AddRuntimeBasedConfiguration(hostContext);
                });
    }
}

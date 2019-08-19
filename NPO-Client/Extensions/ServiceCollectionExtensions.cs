using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NPO_Client.Settings;
using System;

namespace NPO_Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRuntimeBasedConfiguration(this IServiceCollection services, HostBuilderContext hostContext)
        {
            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {
                services.Configure<Mqtt>(hostContext.Configuration.GetSection("LinuxDockerContainer:mqtt"));
                services.Configure<Kafka>(hostContext.Configuration.GetSection("LinuxDockerContainer:kafka"));
            }
            else
            {
                services.Configure<Mqtt>(hostContext.Configuration.GetSection("WindowsService:mqtt"));
                services.Configure<Kafka>(hostContext.Configuration.GetSection("WindowsService:kafka"));
            }
        }
    }
}

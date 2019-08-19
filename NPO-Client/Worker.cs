using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NPO_Client.StreamProcessor;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NPO_Client
{
    public class Worker : IHostedService, IDisposable
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISubscriber _mqtt;

        public Worker(ISubscriber mqtt, ILogger<Worker> logger)
        {
            _logger = logger;
            _mqtt = mqtt;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker started at: {DateTime.Now}");

            await _mqtt.Subscribe();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker stopped at: {DateTime.Now}");

            await _mqtt.Unsubscribe();
            await _mqtt.Disconnect();
        }

        public void Dispose()
        {
            _logger.LogInformation($"Worker disposed at: {DateTime.Now}");

            _mqtt.Dispose();
        }
    }
}

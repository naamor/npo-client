// Based on https://github.com/chkr1011/MQTTnet/wiki/Client

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using NPO_Client.Settings;
using System;
using System.Text;
using System.Threading.Tasks;

namespace NPO_Client.StreamProcessor
{
    public class MqttController : ISubscriber
    {
        private readonly ILogger<MqttController> _logger;
        private readonly IOptions<Mqtt> _configuration;
        private readonly IPublisher _kafka;
        IMqttClientOptions options;
        IMqttClient client;

        public MqttController(ILogger<MqttController> logger, IOptions<Mqtt> configuration, IPublisher kafka)
        {
            _configuration = configuration;
            _logger = logger;
            _kafka = kafka;

            _logger.LogInformation("Create mqtt client");
            var factory = new MqttFactory();
            client = factory.CreateMqttClient();

            _logger.LogInformation("Set client options");
            options = new MqttClientOptionsBuilder()
                .WithClientId("NPO-Client")
                .WithTcpServer(_configuration.Value.Host, _configuration.Value.Port)
                .WithCleanSession()
                .Build();
        }

        private async Task Connect()
        {
            _logger.LogInformation($"Connect to {options.ChannelOptions.ToString()}");
            await client.ConnectAsync(options);
            _logger.LogInformation("Connected");
        }

        public async Task Disconnect()
        {
            _logger.LogInformation("Disconnect from mqtt");
            await client.DisconnectAsync();
            _logger.LogInformation("Disconnected");
        }

        private void Reconnect()
        {
            _logger.LogInformation("Register reconnection");
            client.UseDisconnectedHandler(async e =>
            {
                _logger.LogInformation("Disconnected from Server");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await client.ConnectAsync(options);
                    _logger.LogInformation("Reconnected");
                }
                catch
                {
                    _logger.LogInformation("Reconnecting failed");
                }
            });
            _logger.LogInformation("Reconnection registered");
        }

        public void Dispose()
        {
            _logger.LogInformation("Dispose client");
            client.Dispose();
            _logger.LogInformation("Disposed");
        }

        public async Task Subscribe()
        {
            Reconnect();

            _logger.LogInformation("Register subscription");
            client.UseConnectedHandler(async e =>
            {
                var topicFilter = new TopicFilterBuilder()
                    .WithTopic(_configuration.Value.Topic)
                    .Build();

                _logger.LogInformation($"Subscribe to topic {topicFilter.Topic}");
                await client.SubscribeAsync(topicFilter);
                _logger.LogInformation("Subscribed");
            });
            _logger.LogInformation("Subscription registered");

            ConsumeMessage();
            await Connect();
        }

        public async Task Unsubscribe()
        {
            _logger.LogInformation($"Unsubscribe from topic {_configuration.Value.Topic}");
            await client.UnsubscribeAsync(_configuration.Value.Topic);
            _logger.LogInformation("Unsubscribed");
        }

        private void ConsumeMessage()
        {
            _logger.LogInformation("Register message consumption");
            client.UseApplicationMessageReceivedHandler(e =>
            {
                _logger.LogInformation($"Received message {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Task.Run(() => _kafka.Publish(e.ApplicationMessage.Payload));
            });
            _logger.LogInformation("Message consumption registered");
        }
    }
}
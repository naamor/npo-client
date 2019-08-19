// Based on https://github.com/confluentinc/confluent-kafka-dotnet

using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NPO_Client.Settings;
using System.Threading.Tasks;

namespace NPO_Client.StreamProcessor
{
    public class KafkaController : IPublisher
    {
        private readonly ILogger<KafkaController> _logger;
        private readonly IOptions<Kafka> _configuration;
        ProducerConfig config;

        public KafkaController(ILogger<KafkaController> logger, IOptions<Kafka> configuration)
        {
            _configuration = configuration;
            _logger = logger;

            _logger.LogInformation("Set kafka config");
            config = new ProducerConfig
            {
                BootstrapServers = $"{_configuration.Value.Host}:{_configuration.Value.Port}"
            };
        }

        public async Task Publish(byte[] message)
        {
            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            using (var producer = new ProducerBuilder<Null, byte[]>(config).Build())
            {
                try
                {
                    var result = await producer.ProduceAsync(_configuration.Value.Topic, new Message<Null, byte[]> { Value = message });
                    _logger.LogInformation($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> exception)
                {
                    _logger.LogError($"Delivery failed: {exception.Error.Reason}");
                }
            }
        }
    }
}

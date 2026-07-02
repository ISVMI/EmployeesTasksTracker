using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;
using Shared.Messages;
using System.Text.Json;

namespace Shared.InterfacesImplementation
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly IConfiguration _configuration;

        public KafkaProducer(IProducer<string, string> producer, IConfiguration configuration)
        {
            _producer = producer;
            _configuration = configuration;
        }

        public async Task PublishAsync(TaskDataChanged message)
        {
            var json = JsonSerializer.Serialize(message);

            var fullMessage = new Message<string, string>
            {
                Key = message.TaskId.ToString(),
                Value = json
            };

            await _producer.ProduceAsync(_configuration["Kafka:Topic"], fullMessage);
        }
    }
}

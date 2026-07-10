using Confluent.Kafka;
using EmployeesTasksTracker.HistoryService.Core.Interfaces;
using EmployeesTasksTracker.HistoryService.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Messages;
using System.Text.Json;

namespace EmployeesTasksTracker.HistoryService.Infrastructure.Consumers
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ITaskChangesRepo _repo;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ITaskChangesRepo repo,
            ILogger<KafkaConsumerService> logger)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _repo = repo;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:Host"],
                GroupId = "history-service",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(_configuration["Kafka:Topic"]);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(TimeSpan.FromSeconds(2));

                    if (result is null)
                        continue;

                    var message = JsonSerializer.Deserialize<TaskDataChanged>(result.Message.Value);

                    using var scope = _scopeFactory.CreateScope();

                    var taskChanges = new TaskChanges
                    {
                        TaskId = message.TaskId,
                        ChangedAt = message.ChangedAt,
                        Changes = message.Changes.ToList()
                    };

                    await _repo.CreateTaskChangesRecord(taskChanges, cancellationToken);

                    consumer.Commit(result);
                }
                catch (ConsumeException ex)
                    when (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
                {
                    _logger.LogWarning("Topic doesn't exist yet. Waiting...");

                    await Task.Delay(2000, cancellationToken);
                }
            }
        }
    }
}

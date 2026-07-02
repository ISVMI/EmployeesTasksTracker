using Shared.Messages;

namespace Shared.Interfaces
{
    public interface IKafkaProducer
    {
        Task PublishAsync(TaskDataChanged message);
    }
}

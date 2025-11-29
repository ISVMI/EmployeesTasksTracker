using Shared.DTOs;

namespace Shared.Messages
{
    public record TaskCreated(Guid TaskId, TaskForReportDTO CreatedTask, DateTime CreatedAt);
}

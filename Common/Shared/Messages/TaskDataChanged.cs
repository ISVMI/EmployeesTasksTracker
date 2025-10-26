namespace Shared.Messages
{
    public sealed record TaskDataChanged(Guid taskId, IEnumerable<string> Changes);
}

namespace Shared.Messages
{
    public sealed record TaskDataChanged(Guid TaskId, IEnumerable<string> Changes, DateTime ChangedAt);
}

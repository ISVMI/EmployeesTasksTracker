namespace Shared.Messages
{
    public record TaskDeleted(Guid DeletedTaskId, DateTime DeletedAt);
}

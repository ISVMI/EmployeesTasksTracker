namespace Shared.Interfaces
{
    public interface IIdsGetter
    {
        Task<IEnumerable<Guid>> GetAllIds(CancellationToken cancellationToken = default);
    }
}

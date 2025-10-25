namespace Shared.Interfaces
{
    public interface IPdfReportGenerator
    {
        Task<byte[]> GenerateReportAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

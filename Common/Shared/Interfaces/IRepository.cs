namespace Shared.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<Guid> CreateAsync(TEntity entity, CancellationToken token = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken token = default);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken token = default);
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);
    }
}

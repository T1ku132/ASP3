using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);

    // track: true — для изменения; false — для чтения (быстрее, без отслеживания)
    Task<Order?> GetByIdAsync(int id, CancellationToken ct, bool track = true);
    Task<bool> ExistsAsync(int id, CancellationToken ct);

    Task AddAsync(Order entity, CancellationToken ct);
    void Update(Order entity);
    void Remove(Order entity);
    Task SaveChangesAsync(CancellationToken ct);
}

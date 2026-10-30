using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

public interface IOrderItemRepository
{
    Task<List<OrderItem>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);

    // track: true — для изменения; false — для чтения (быстрее, без отслеживания)
    Task<OrderItem?> GetByIdAsync(int id, CancellationToken ct, bool track = true);
    Task<bool> ExistsAsync(int id, CancellationToken ct);

    Task AddAsync(OrderItem entity, CancellationToken ct);
    void Update(OrderItem entity);
    void Remove(OrderItem entity);
    Task SaveChangesAsync(CancellationToken ct);
}

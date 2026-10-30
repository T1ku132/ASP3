using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

public interface ICartItemRepository
{
    Task<List<CartItem>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);

    // track: true — для изменения; false — для чтения (быстрее, без отслеживания)
    Task<CartItem?> GetByIdAsync(int id, CancellationToken ct, bool track = true);
    Task<bool> ExistsAsync(int id, CancellationToken ct);

    Task AddAsync(CartItem entity, CancellationToken ct);
    void Update(CartItem entity);
    void Remove(CartItem entity);
    Task SaveChangesAsync(CancellationToken ct);

    Task<bool> PairExistsAsync(int cartId, int productId, int excludeId, CancellationToken ct);
}

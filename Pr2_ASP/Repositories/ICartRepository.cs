using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

public interface ICartRepository
{
    Task<List<Cart>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);

    // track: true — для изменения; false — для чтения (быстрее, без отслеживания)
    Task<Cart?> GetByIdAsync(int id, CancellationToken ct, bool track = true);
    Task<bool> ExistsAsync(int id, CancellationToken ct);

    Task AddAsync(Cart entity, CancellationToken ct);
    void Update(Cart entity);
    void Remove(Cart entity);
    Task SaveChangesAsync(CancellationToken ct);

    Task<bool> ExistsForUserAsync(int userId, int excludeCartId, CancellationToken ct);
}

using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);

    // track: true — для изменения; false — для чтения (быстрее, без отслеживания)
    Task<Product?> GetByIdAsync(int id, CancellationToken ct, bool track = true);
    Task<bool> ExistsAsync(int id, CancellationToken ct);

    Task AddAsync(Product entity, CancellationToken ct);
    void Update(Product entity);
    void Remove(Product entity);
    Task SaveChangesAsync(CancellationToken ct);

    Task<bool> HasDependentsAsync(int id, CancellationToken ct);
}

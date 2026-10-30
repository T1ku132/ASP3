using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);

    // track: true — для изменения; false — для чтения (быстрее, без отслеживания)
    Task<Category?> GetByIdAsync(int id, CancellationToken ct, bool track = true);
    Task<bool> ExistsAsync(int id, CancellationToken ct);

    Task AddAsync(Category entity, CancellationToken ct);
    void Update(Category entity);
    void Remove(Category entity);
    Task SaveChangesAsync(CancellationToken ct);

    Task<bool> HasDependentsAsync(int id, CancellationToken ct);
}

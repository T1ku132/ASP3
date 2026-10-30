using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

public interface IRoleRepository
{
    Task<List<Role>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);

    // track: true — для изменения; false — для чтения (быстрее, без отслеживания)
    Task<Role?> GetByIdAsync(int id, CancellationToken ct, bool track = true);
    Task<bool> ExistsAsync(int id, CancellationToken ct);

    Task AddAsync(Role entity, CancellationToken ct);
    void Update(Role entity);
    void Remove(Role entity);
    Task SaveChangesAsync(CancellationToken ct);

    Task<bool> NameExistsAsync(string name, int excludeId, CancellationToken ct);
    Task<bool> HasDependentsAsync(int id, CancellationToken ct);
}

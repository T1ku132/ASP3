using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);

    // track: true — для изменения; false — для чтения (быстрее, без отслеживания)
    Task<User?> GetByIdAsync(int id, CancellationToken ct, bool track = true);
    Task<bool> ExistsAsync(int id, CancellationToken ct);

    Task AddAsync(User entity, CancellationToken ct);
    void Update(User entity);
    void Remove(User entity);
    Task SaveChangesAsync(CancellationToken ct);

    Task<bool> UsernameExistsAsync(string username, int excludeId, CancellationToken ct);
    Task<bool> EmailExistsAsync(string email, int excludeId, CancellationToken ct);
    Task<bool> HasDependentsAsync(int id, CancellationToken ct);
}

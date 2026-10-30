using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

public interface IReservationRepository
{
    Task<List<Reservation>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);

    // track: true — для изменения; false — для чтения (быстрее, без отслеживания)
    Task<Reservation?> GetByIdAsync(int id, CancellationToken ct, bool track = true);
    Task<bool> ExistsAsync(int id, CancellationToken ct);

    Task AddAsync(Reservation entity, CancellationToken ct);
    void Update(Reservation entity);
    void Remove(Reservation entity);
    Task SaveChangesAsync(CancellationToken ct);

    Task<Reservation?> GetActiveByIdAsync(int id, CancellationToken ct);
}

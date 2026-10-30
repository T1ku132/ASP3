using Microsoft.EntityFrameworkCore;
using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

// Единственный слой, которому разрешено видеть ShopDBContext.
// Только CRUD и простые запросы — бизнес-логики здесь нет.
public class ReservationRepository : IReservationRepository
{
    private readonly ShopDBContext _context;

    public ReservationRepository(ShopDBContext context)
    {
        _context = context;
    }

    private IQueryable<Reservation> Query() =>
        _context.Reservations
            .Include(r => r.User).Include(r => r.Product);

    public async Task<List<Reservation>> GetPageAsync(int page, int pageSize, CancellationToken ct) =>
        await Query()
            .AsNoTracking()
            .OrderBy(x => x.ReservationId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Reservations.CountAsync(ct);

    public async Task<Reservation?> GetByIdAsync(int id, CancellationToken ct, bool track = true)
    {
        var query = Query();
        if (!track) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.ReservationId == id, ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct) =>
        await _context.Reservations.AnyAsync(x => x.ReservationId == id, ct);

    public async Task AddAsync(Reservation entity, CancellationToken ct) =>
        await _context.Reservations.AddAsync(entity, ct);

    public void Update(Reservation entity) =>
        _context.Entry(entity).State = EntityState.Modified;

    public void Remove(Reservation entity)
    {
        _context.Reservations.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken ct) =>
        await _context.SaveChangesAsync(ct);

    // Активная запись = ещё не возвращена (ReleasedAt == null). Загружается с отслеживанием,
    // вместе с Product — чтобы сервис мог вернуть остаток и закрыть запись одним SaveChangesAsync.
    public async Task<Reservation?> GetActiveByIdAsync(int id, CancellationToken ct) =>
        await Query().FirstOrDefaultAsync(r => r.ReservationId == id && r.ReleasedAt == null, ct);
}

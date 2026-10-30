using Microsoft.EntityFrameworkCore;
using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

// Единственный слой, которому разрешено видеть ShopDBContext.
// Только CRUD и простые запросы — бизнес-логики здесь нет.
public class UserRepository : IUserRepository
{
    private readonly ShopDBContext _context;

    public UserRepository(ShopDBContext context)
    {
        _context = context;
    }

    private IQueryable<User> Query() =>
        _context.Users
            .Include(u => u.Role);

    public async Task<List<User>> GetPageAsync(int page, int pageSize, CancellationToken ct) =>
        await Query()
            .AsNoTracking()
            .OrderBy(x => x.UserId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Users.CountAsync(ct);

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct, bool track = true)
    {
        var query = Query();
        if (!track) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.UserId == id, ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct) =>
        await _context.Users.AnyAsync(x => x.UserId == id, ct);

    public async Task AddAsync(User entity, CancellationToken ct) =>
        await _context.Users.AddAsync(entity, ct);

    public void Update(User entity) =>
        _context.Entry(entity).State = EntityState.Modified;

    public void Remove(User entity)
    {
        _context.Users.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken ct) =>
        await _context.SaveChangesAsync(ct);

    public async Task<bool> UsernameExistsAsync(string username, int excludeId, CancellationToken ct) =>
        await _context.Users.AnyAsync(u => u.Username == username && u.UserId != excludeId, ct);

    public async Task<bool> EmailExistsAsync(string email, int excludeId, CancellationToken ct) =>
        await _context.Users.AnyAsync(u => u.Email == email && u.UserId != excludeId, ct);

    public async Task<bool> HasDependentsAsync(int id, CancellationToken ct) =>
        await _context.Carts.AnyAsync(c => c.UserId == id, ct)
        || await _context.Orders.AnyAsync(o => o.UserId == id, ct)
        || await _context.Reservations.AnyAsync(r => r.UserId == id, ct);
}

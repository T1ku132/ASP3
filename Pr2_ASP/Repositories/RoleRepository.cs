using Microsoft.EntityFrameworkCore;
using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

// Единственный слой, которому разрешено видеть ShopDBContext.
// Только CRUD и простые запросы — бизнес-логики здесь нет.
public class RoleRepository : IRoleRepository
{
    private readonly ShopDBContext _context;

    public RoleRepository(ShopDBContext context)
    {
        _context = context;
    }

    private IQueryable<Role> Query() =>
        _context.Roles
            ;

    public async Task<List<Role>> GetPageAsync(int page, int pageSize, CancellationToken ct) =>
        await Query()
            .AsNoTracking()
            .OrderBy(x => x.RoleId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Roles.CountAsync(ct);

    public async Task<Role?> GetByIdAsync(int id, CancellationToken ct, bool track = true)
    {
        var query = Query();
        if (!track) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.RoleId == id, ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct) =>
        await _context.Roles.AnyAsync(x => x.RoleId == id, ct);

    public async Task AddAsync(Role entity, CancellationToken ct) =>
        await _context.Roles.AddAsync(entity, ct);

    public void Update(Role entity) =>
        _context.Entry(entity).State = EntityState.Modified;

    public void Remove(Role entity)
    {
        _context.Roles.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken ct) =>
        await _context.SaveChangesAsync(ct);

    public async Task<bool> NameExistsAsync(string name, int excludeId, CancellationToken ct) =>
        await _context.Roles.AnyAsync(r => r.RoleName == name && r.RoleId != excludeId, ct);

    public async Task<bool> HasDependentsAsync(int id, CancellationToken ct) =>
        await _context.Users.AnyAsync(u => u.RoleId == id, ct);
}

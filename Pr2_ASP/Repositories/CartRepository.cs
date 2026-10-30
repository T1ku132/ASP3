using Microsoft.EntityFrameworkCore;
using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

// Единственный слой, которому разрешено видеть ShopDBContext.
// Только CRUD и простые запросы — бизнес-логики здесь нет.
public class CartRepository : ICartRepository
{
    private readonly ShopDBContext _context;

    public CartRepository(ShopDBContext context)
    {
        _context = context;
    }

    private IQueryable<Cart> Query() =>
        _context.Carts
            .Include(c => c.User).Include(c => c.CartItems).ThenInclude(i => i.Product);

    public async Task<List<Cart>> GetPageAsync(int page, int pageSize, CancellationToken ct) =>
        await Query()
            .AsNoTracking()
            .OrderBy(x => x.CartId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Carts.CountAsync(ct);

    public async Task<Cart?> GetByIdAsync(int id, CancellationToken ct, bool track = true)
    {
        var query = Query();
        if (!track) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.CartId == id, ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct) =>
        await _context.Carts.AnyAsync(x => x.CartId == id, ct);

    public async Task AddAsync(Cart entity, CancellationToken ct) =>
        await _context.Carts.AddAsync(entity, ct);

    public void Update(Cart entity) =>
        _context.Entry(entity).State = EntityState.Modified;

    public void Remove(Cart entity)
    {
        _context.CartItems.RemoveRange(entity.CartItems);
        _context.Carts.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken ct) =>
        await _context.SaveChangesAsync(ct);

    public async Task<bool> ExistsForUserAsync(int userId, int excludeCartId, CancellationToken ct) =>
        await _context.Carts.AnyAsync(c => c.UserId == userId && c.CartId != excludeCartId, ct);
}

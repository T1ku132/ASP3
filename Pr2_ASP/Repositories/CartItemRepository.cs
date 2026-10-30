using Microsoft.EntityFrameworkCore;
using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

// Единственный слой, которому разрешено видеть ShopDBContext.
// Только CRUD и простые запросы — бизнес-логики здесь нет.
public class CartItemRepository : ICartItemRepository
{
    private readonly ShopDBContext _context;

    public CartItemRepository(ShopDBContext context)
    {
        _context = context;
    }

    private IQueryable<CartItem> Query() =>
        _context.CartItems
            .Include(i => i.Product);

    public async Task<List<CartItem>> GetPageAsync(int page, int pageSize, CancellationToken ct) =>
        await Query()
            .AsNoTracking()
            .OrderBy(x => x.CartItemId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.CartItems.CountAsync(ct);

    public async Task<CartItem?> GetByIdAsync(int id, CancellationToken ct, bool track = true)
    {
        var query = Query();
        if (!track) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.CartItemId == id, ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct) =>
        await _context.CartItems.AnyAsync(x => x.CartItemId == id, ct);

    public async Task AddAsync(CartItem entity, CancellationToken ct) =>
        await _context.CartItems.AddAsync(entity, ct);

    public void Update(CartItem entity) =>
        _context.Entry(entity).State = EntityState.Modified;

    public void Remove(CartItem entity)
    {
        _context.CartItems.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken ct) =>
        await _context.SaveChangesAsync(ct);

    public async Task<bool> PairExistsAsync(int cartId, int productId, int excludeId, CancellationToken ct) =>
        await _context.CartItems.AnyAsync(i => i.CartId == cartId && i.ProductId == productId && i.CartItemId != excludeId, ct);
}

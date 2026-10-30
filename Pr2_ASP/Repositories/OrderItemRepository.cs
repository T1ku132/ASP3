using Microsoft.EntityFrameworkCore;
using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

// Единственный слой, которому разрешено видеть ShopDBContext.
// Только CRUD и простые запросы — бизнес-логики здесь нет.
public class OrderItemRepository : IOrderItemRepository
{
    private readonly ShopDBContext _context;

    public OrderItemRepository(ShopDBContext context)
    {
        _context = context;
    }

    private IQueryable<OrderItem> Query() =>
        _context.OrderItems
            .Include(i => i.Product);

    public async Task<List<OrderItem>> GetPageAsync(int page, int pageSize, CancellationToken ct) =>
        await Query()
            .AsNoTracking()
            .OrderBy(x => x.OrderItemId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.OrderItems.CountAsync(ct);

    public async Task<OrderItem?> GetByIdAsync(int id, CancellationToken ct, bool track = true)
    {
        var query = Query();
        if (!track) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.OrderItemId == id, ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct) =>
        await _context.OrderItems.AnyAsync(x => x.OrderItemId == id, ct);

    public async Task AddAsync(OrderItem entity, CancellationToken ct) =>
        await _context.OrderItems.AddAsync(entity, ct);

    public void Update(OrderItem entity) =>
        _context.Entry(entity).State = EntityState.Modified;

    public void Remove(OrderItem entity)
    {
        _context.OrderItems.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken ct) =>
        await _context.SaveChangesAsync(ct);
}

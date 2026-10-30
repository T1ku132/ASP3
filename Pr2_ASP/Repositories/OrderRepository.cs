using Microsoft.EntityFrameworkCore;
using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

// Единственный слой, которому разрешено видеть ShopDBContext.
// Только CRUD и простые запросы — бизнес-логики здесь нет.
public class OrderRepository : IOrderRepository
{
    private readonly ShopDBContext _context;

    public OrderRepository(ShopDBContext context)
    {
        _context = context;
    }

    private IQueryable<Order> Query() =>
        _context.Orders
            .Include(o => o.User).Include(o => o.OrderItems).ThenInclude(i => i.Product);

    public async Task<List<Order>> GetPageAsync(int page, int pageSize, CancellationToken ct) =>
        await Query()
            .AsNoTracking()
            .OrderBy(x => x.OrderId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Orders.CountAsync(ct);

    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct, bool track = true)
    {
        var query = Query();
        if (!track) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.OrderId == id, ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct) =>
        await _context.Orders.AnyAsync(x => x.OrderId == id, ct);

    public async Task AddAsync(Order entity, CancellationToken ct) =>
        await _context.Orders.AddAsync(entity, ct);

    public void Update(Order entity) =>
        _context.Entry(entity).State = EntityState.Modified;

    public void Remove(Order entity)
    {
        _context.OrderItems.RemoveRange(entity.OrderItems);
        _context.Orders.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken ct) =>
        await _context.SaveChangesAsync(ct);
}

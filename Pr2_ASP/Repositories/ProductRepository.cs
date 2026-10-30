using Microsoft.EntityFrameworkCore;
using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

// Единственный слой, которому разрешено видеть ShopDBContext.
// Только CRUD и простые запросы — бизнес-логики здесь нет.
public class ProductRepository : IProductRepository
{
    private readonly ShopDBContext _context;

    public ProductRepository(ShopDBContext context)
    {
        _context = context;
    }

    private IQueryable<Product> Query() =>
        _context.Products
            .Include(p => p.Category);

    public async Task<List<Product>> GetPageAsync(int page, int pageSize, CancellationToken ct) =>
        await Query()
            .AsNoTracking()
            .OrderBy(x => x.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Products.CountAsync(ct);

    public async Task<Product?> GetByIdAsync(int id, CancellationToken ct, bool track = true)
    {
        var query = Query();
        if (!track) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.ProductId == id, ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct) =>
        await _context.Products.AnyAsync(x => x.ProductId == id, ct);

    public async Task AddAsync(Product entity, CancellationToken ct) =>
        await _context.Products.AddAsync(entity, ct);

    public void Update(Product entity) =>
        _context.Entry(entity).State = EntityState.Modified;

    public void Remove(Product entity)
    {
        _context.Products.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken ct) =>
        await _context.SaveChangesAsync(ct);

    public async Task<bool> HasDependentsAsync(int id, CancellationToken ct) =>
        await _context.CartItems.AnyAsync(i => i.ProductId == id, ct)
        || await _context.OrderItems.AnyAsync(i => i.ProductId == id, ct)
        || await _context.Reservations.AnyAsync(r => r.ProductId == id, ct);
}

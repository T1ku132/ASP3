using Microsoft.EntityFrameworkCore;
using Pr1_ASP.Models;

namespace Pr1_ASP.Repositories;

// Единственный слой, которому разрешено видеть ShopDBContext.
// Только CRUD и простые запросы — бизнес-логики здесь нет.
public class CategoryRepository : ICategoryRepository
{
    private readonly ShopDBContext _context;

    public CategoryRepository(ShopDBContext context)
    {
        _context = context;
    }

    private IQueryable<Category> Query() =>
        _context.Categories
            .Include(c => c.ParentCategory);

    public async Task<List<Category>> GetPageAsync(int page, int pageSize, CancellationToken ct) =>
        await Query()
            .AsNoTracking()
            .OrderBy(x => x.CategoryId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Categories.CountAsync(ct);

    public async Task<Category?> GetByIdAsync(int id, CancellationToken ct, bool track = true)
    {
        var query = Query();
        if (!track) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.CategoryId == id, ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct) =>
        await _context.Categories.AnyAsync(x => x.CategoryId == id, ct);

    public async Task AddAsync(Category entity, CancellationToken ct) =>
        await _context.Categories.AddAsync(entity, ct);

    public void Update(Category entity) =>
        _context.Entry(entity).State = EntityState.Modified;

    public void Remove(Category entity)
    {
        _context.Categories.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken ct) =>
        await _context.SaveChangesAsync(ct);

    public async Task<bool> HasDependentsAsync(int id, CancellationToken ct) =>
        await _context.Products.AnyAsync(p => p.CategoryId == id, ct)
        || await _context.Categories.AnyAsync(c => c.ParentCategoryId == id, ct);
}

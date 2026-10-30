using AutoMapper;
using Pr1_ASP.Dtos;
using Pr1_ASP.Models;
using Pr1_ASP.Repositories;

namespace Pr1_ASP.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categories;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, ICategoryRepository categories, IMapper mapper)
    {
        _repository = repository;
        _categories = categories;
        _mapper = mapper;
    }

    public async Task<(List<ProductDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var entities = await _repository.GetPageAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<ProductDto>>(entities), total);
    }

    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct, track: false);
        return entity is null ? null : _mapper.Map<ProductDto>(entity);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken ct)
    {
        var entity = _mapper.Map<Product>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        await ValidateAsync(entity, ct);

        await _repository.AddAsync(entity, ct);
        await _repository.SaveChangesAsync(ct);

        return await LoadAsync(entity.ProductId, ct);
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null) return null;

        _mapper.Map(dto, entity);

        await ValidateAsync(entity, ct);

        _repository.Update(entity);
        await _repository.SaveChangesAsync(ct);

        return await LoadAsync(id, ct);
    }

    // PATCH: профиль маппинга копирует только непустые поля DTO (см. MappingProfile)
    public async Task<ProductDto?> PatchAsync(int id, PatchProductDto dto, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null) return null;

        _mapper.Map(dto, entity);

        await ValidateAsync(entity, ct);

        _repository.Update(entity);
        await _repository.SaveChangesAsync(ct);

        return await LoadAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null) return false;

        if (await _repository.HasDependentsAsync(id, ct))
            throw new ConflictException("Нельзя удалить товар: он есть в корзинах, заказах или резервированиях.");
        _repository.Remove(entity);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    // Перечитываем без отслеживания, чтобы в ответе были актуальные вложенные данные
    private async Task<ProductDto> LoadAsync(int id, CancellationToken ct)
    {
        var saved = await _repository.GetByIdAsync(id, ct, track: false)
            ?? throw new InvalidOperationException("Не удалось загрузить сохранённую запись.");
        return _mapper.Map<ProductDto>(saved);
    }

    private async Task ValidateAsync(Product entity, CancellationToken ct)
    {
        if (!await _categories.ExistsAsync(entity.CategoryId, ct))
            throw new InvalidOperationException($"Категория с Id {entity.CategoryId} не найдена.");
    }
}

using AutoMapper;
using Pr1_ASP.Dtos;
using Pr1_ASP.Models;
using Pr1_ASP.Repositories;

namespace Pr1_ASP.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;

        _mapper = mapper;
    }

    public async Task<(List<CategoryDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var entities = await _repository.GetPageAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<CategoryDto>>(entities), total);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct, track: false);
        return entity is null ? null : _mapper.Map<CategoryDto>(entity);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct)
    {
        var entity = _mapper.Map<Category>(dto);

        await ValidateAsync(entity, ct);

        await _repository.AddAsync(entity, ct);
        await _repository.SaveChangesAsync(ct);

        return await LoadAsync(entity.CategoryId, ct);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct)
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
    public async Task<CategoryDto?> PatchAsync(int id, PatchCategoryDto dto, CancellationToken ct)
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
            throw new ConflictException("Нельзя удалить категорию: в ней есть товары или подкатегории.");
        _repository.Remove(entity);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    // Перечитываем без отслеживания, чтобы в ответе были актуальные вложенные данные
    private async Task<CategoryDto> LoadAsync(int id, CancellationToken ct)
    {
        var saved = await _repository.GetByIdAsync(id, ct, track: false)
            ?? throw new InvalidOperationException("Не удалось загрузить сохранённую запись.");
        return _mapper.Map<CategoryDto>(saved);
    }

    private async Task ValidateAsync(Category entity, CancellationToken ct)
    {
        if (entity.ParentCategoryId is not null)
        {
            if (entity.ParentCategoryId == entity.CategoryId)
                throw new InvalidOperationException("Категория не может быть родителем самой себя.");
            if (!await _repository.ExistsAsync(entity.ParentCategoryId.Value, ct))
                throw new InvalidOperationException($"Родительская категория с Id {entity.ParentCategoryId} не найдена.");
        }
    }
}

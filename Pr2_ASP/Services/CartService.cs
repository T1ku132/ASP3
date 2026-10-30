using AutoMapper;
using Pr1_ASP.Dtos;
using Pr1_ASP.Models;
using Pr1_ASP.Repositories;

namespace Pr1_ASP.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _repository;
    private readonly IUserRepository _users;
    private readonly IMapper _mapper;

    public CartService(ICartRepository repository, IUserRepository users, IMapper mapper)
    {
        _repository = repository;
        _users = users;
        _mapper = mapper;
    }

    public async Task<(List<CartDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var entities = await _repository.GetPageAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<CartDto>>(entities), total);
    }

    public async Task<CartDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct, track: false);
        return entity is null ? null : _mapper.Map<CartDto>(entity);
    }

    public async Task<CartDto> CreateAsync(CreateCartDto dto, CancellationToken ct)
    {
        var entity = _mapper.Map<Cart>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.LastUpdated = DateTime.UtcNow;
        await ValidateAsync(entity, ct);

        await _repository.AddAsync(entity, ct);
        await _repository.SaveChangesAsync(ct);

        return await LoadAsync(entity.CartId, ct);
    }

    public async Task<CartDto?> UpdateAsync(int id, UpdateCartDto dto, CancellationToken ct)
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
    public async Task<CartDto?> PatchAsync(int id, PatchCartDto dto, CancellationToken ct)
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

        _repository.Remove(entity);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    // Перечитываем без отслеживания, чтобы в ответе были актуальные вложенные данные
    private async Task<CartDto> LoadAsync(int id, CancellationToken ct)
    {
        var saved = await _repository.GetByIdAsync(id, ct, track: false)
            ?? throw new InvalidOperationException("Не удалось загрузить сохранённую запись.");
        return _mapper.Map<CartDto>(saved);
    }

    private async Task ValidateAsync(Cart entity, CancellationToken ct)
    {
        if (!await _users.ExistsAsync(entity.UserId, ct))
            throw new InvalidOperationException($"Пользователь с Id {entity.UserId} не найден.");
        if (await _repository.ExistsForUserAsync(entity.UserId, entity.CartId, ct))
            throw new ConflictException($"У пользователя {entity.UserId} уже есть корзина.");
    }
}

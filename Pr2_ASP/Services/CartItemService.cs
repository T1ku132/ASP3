using AutoMapper;
using Pr1_ASP.Dtos;
using Pr1_ASP.Models;
using Pr1_ASP.Repositories;

namespace Pr1_ASP.Services;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository _repository;
    private readonly ICartRepository _carts;
    private readonly IProductRepository _products;
    private readonly IMapper _mapper;

    public CartItemService(ICartItemRepository repository, ICartRepository carts, IProductRepository products, IMapper mapper)
    {
        _repository = repository;
        _carts = carts;
        _products = products;
        _mapper = mapper;
    }

    public async Task<(List<CartItemDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var entities = await _repository.GetPageAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<CartItemDto>>(entities), total);
    }

    public async Task<CartItemDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct, track: false);
        return entity is null ? null : _mapper.Map<CartItemDto>(entity);
    }

    public async Task<CartItemDto> CreateAsync(CreateCartItemDto dto, CancellationToken ct)
    {
        var entity = _mapper.Map<CartItem>(dto);
        entity.AddedAt = DateTime.UtcNow;
        await ValidateAsync(entity, ct);

        await _repository.AddAsync(entity, ct);
        await _repository.SaveChangesAsync(ct);

        return await LoadAsync(entity.CartItemId, ct);
    }

    public async Task<CartItemDto?> UpdateAsync(int id, UpdateCartItemDto dto, CancellationToken ct)
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
    public async Task<CartItemDto?> PatchAsync(int id, PatchCartItemDto dto, CancellationToken ct)
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
    private async Task<CartItemDto> LoadAsync(int id, CancellationToken ct)
    {
        var saved = await _repository.GetByIdAsync(id, ct, track: false)
            ?? throw new InvalidOperationException("Не удалось загрузить сохранённую запись.");
        return _mapper.Map<CartItemDto>(saved);
    }

    private async Task ValidateAsync(CartItem entity, CancellationToken ct)
    {
        if (!await _carts.ExistsAsync(entity.CartId, ct))
            throw new InvalidOperationException($"Корзина с Id {entity.CartId} не найдена.");
        if (!await _products.ExistsAsync(entity.ProductId, ct))
            throw new InvalidOperationException($"Товар с Id {entity.ProductId} не найден.");
        if (entity.Quantity < 1)
            throw new InvalidOperationException("Количество должно быть не меньше 1.");
        if (await _repository.PairExistsAsync(entity.CartId, entity.ProductId, entity.CartItemId, ct))
            throw new ConflictException("Этот товар уже есть в корзине — измените количество существующей позиции.");
    }
}

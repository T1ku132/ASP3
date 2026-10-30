using AutoMapper;
using Pr1_ASP.Dtos;
using Pr1_ASP.Models;
using Pr1_ASP.Repositories;

namespace Pr1_ASP.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _repository;
    private readonly IOrderRepository _orders;
    private readonly IProductRepository _products;
    private readonly IMapper _mapper;

    public OrderItemService(IOrderItemRepository repository, IOrderRepository orders, IProductRepository products, IMapper mapper)
    {
        _repository = repository;
        _orders = orders;
        _products = products;
        _mapper = mapper;
    }

    public async Task<(List<OrderItemDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var entities = await _repository.GetPageAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<OrderItemDto>>(entities), total);
    }

    public async Task<OrderItemDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct, track: false);
        return entity is null ? null : _mapper.Map<OrderItemDto>(entity);
    }

    public async Task<OrderItemDto> CreateAsync(CreateOrderItemDto dto, CancellationToken ct)
    {
        var entity = _mapper.Map<OrderItem>(dto);

        await ValidateAsync(entity, ct);

        await _repository.AddAsync(entity, ct);
        await _repository.SaveChangesAsync(ct);

        return await LoadAsync(entity.OrderItemId, ct);
    }

    public async Task<OrderItemDto?> UpdateAsync(int id, UpdateOrderItemDto dto, CancellationToken ct)
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
    public async Task<OrderItemDto?> PatchAsync(int id, PatchOrderItemDto dto, CancellationToken ct)
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
    private async Task<OrderItemDto> LoadAsync(int id, CancellationToken ct)
    {
        var saved = await _repository.GetByIdAsync(id, ct, track: false)
            ?? throw new InvalidOperationException("Не удалось загрузить сохранённую запись.");
        return _mapper.Map<OrderItemDto>(saved);
    }

    private async Task ValidateAsync(OrderItem entity, CancellationToken ct)
    {
        if (!await _orders.ExistsAsync(entity.OrderId, ct))
            throw new InvalidOperationException($"Заказ с Id {entity.OrderId} не найден.");
        if (!await _products.ExistsAsync(entity.ProductId, ct))
            throw new InvalidOperationException($"Товар с Id {entity.ProductId} не найден.");
        if (entity.Quantity < 1)
            throw new InvalidOperationException("Количество должно быть не меньше 1.");
    }
}

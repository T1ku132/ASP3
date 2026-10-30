using Pr1_ASP.Dtos;

namespace Pr1_ASP.Services;

// Service ничего не знает про HTTP — ни ActionResult, ни статус-кодов.
// Not-found выражается через null/false, ошибки правил — через исключения
// (InvalidOperationException, KeyNotFoundException, ConflictException); их переводит Controller.
public interface IOrderItemService
{
    Task<(List<OrderItemDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<OrderItemDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<OrderItemDto> CreateAsync(CreateOrderItemDto dto, CancellationToken ct);
    Task<OrderItemDto?> UpdateAsync(int id, UpdateOrderItemDto dto, CancellationToken ct);
    Task<OrderItemDto?> PatchAsync(int id, PatchOrderItemDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

using Pr1_ASP.Dtos;

namespace Pr1_ASP.Services;

// Service ничего не знает про HTTP — ни ActionResult, ни статус-кодов.
// Not-found выражается через null/false, ошибки правил — через исключения
// (InvalidOperationException, KeyNotFoundException, ConflictException); их переводит Controller.
public interface IOrderService
{
    Task<(List<OrderDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<OrderDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<OrderDto> CreateAsync(CreateOrderDto dto, CancellationToken ct);
    Task<OrderDto?> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken ct);
    Task<OrderDto?> PatchAsync(int id, PatchOrderDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

using Pr1_ASP.Dtos;

namespace Pr1_ASP.Services;

// Service ничего не знает про HTTP — ни ActionResult, ни статус-кодов.
// Not-found выражается через null/false, ошибки правил — через исключения
// (InvalidOperationException, KeyNotFoundException, ConflictException); их переводит Controller.
public interface ICartItemService
{
    Task<(List<CartItemDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<CartItemDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<CartItemDto> CreateAsync(CreateCartItemDto dto, CancellationToken ct);
    Task<CartItemDto?> UpdateAsync(int id, UpdateCartItemDto dto, CancellationToken ct);
    Task<CartItemDto?> PatchAsync(int id, PatchCartItemDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

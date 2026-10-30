using Pr1_ASP.Dtos;

namespace Pr1_ASP.Services;

// Service ничего не знает про HTTP — ни ActionResult, ни статус-кодов.
// Not-found выражается через null/false, ошибки правил — через исключения
// (InvalidOperationException, KeyNotFoundException, ConflictException); их переводит Controller.
public interface ICartService
{
    Task<(List<CartDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<CartDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<CartDto> CreateAsync(CreateCartDto dto, CancellationToken ct);
    Task<CartDto?> UpdateAsync(int id, UpdateCartDto dto, CancellationToken ct);
    Task<CartDto?> PatchAsync(int id, PatchCartDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

using Pr1_ASP.Dtos;

namespace Pr1_ASP.Services;

// Service ничего не знает про HTTP — ни ActionResult, ни статус-кодов.
// Not-found выражается через null/false, ошибки правил — через исключения
// (InvalidOperationException, KeyNotFoundException, ConflictException); их переводит Controller.
public interface ICategoryService
{
    Task<(List<CategoryDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct);
    Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct);
    Task<CategoryDto?> PatchAsync(int id, PatchCategoryDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

using Pr1_ASP.Dtos;

namespace Pr1_ASP.Services;

// Service ничего не знает про HTTP — ни ActionResult, ни статус-кодов.
// Not-found выражается через null/false, ошибки правил — через исключения
// (InvalidOperationException, KeyNotFoundException, ConflictException); их переводит Controller.
public interface IRoleService
{
    Task<(List<RoleDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<RoleDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<RoleDto> CreateAsync(CreateRoleDto dto, CancellationToken ct);
    Task<RoleDto?> UpdateAsync(int id, UpdateRoleDto dto, CancellationToken ct);
    Task<RoleDto?> PatchAsync(int id, PatchRoleDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

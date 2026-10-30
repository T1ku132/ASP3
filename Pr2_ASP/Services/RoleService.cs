using AutoMapper;
using Pr1_ASP.Dtos;
using Pr1_ASP.Models;
using Pr1_ASP.Repositories;

namespace Pr1_ASP.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;

    private readonly IMapper _mapper;

    public RoleService(IRoleRepository repository, IMapper mapper)
    {
        _repository = repository;

        _mapper = mapper;
    }

    public async Task<(List<RoleDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var entities = await _repository.GetPageAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<RoleDto>>(entities), total);
    }

    public async Task<RoleDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct, track: false);
        return entity is null ? null : _mapper.Map<RoleDto>(entity);
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto, CancellationToken ct)
    {
        var entity = _mapper.Map<Role>(dto);

        await ValidateAsync(entity, ct);

        await _repository.AddAsync(entity, ct);
        await _repository.SaveChangesAsync(ct);

        return await LoadAsync(entity.RoleId, ct);
    }

    public async Task<RoleDto?> UpdateAsync(int id, UpdateRoleDto dto, CancellationToken ct)
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
    public async Task<RoleDto?> PatchAsync(int id, PatchRoleDto dto, CancellationToken ct)
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
            throw new ConflictException("Нельзя удалить роль: есть пользователи с этой ролью.");
        _repository.Remove(entity);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    // Перечитываем без отслеживания, чтобы в ответе были актуальные вложенные данные
    private async Task<RoleDto> LoadAsync(int id, CancellationToken ct)
    {
        var saved = await _repository.GetByIdAsync(id, ct, track: false)
            ?? throw new InvalidOperationException("Не удалось загрузить сохранённую запись.");
        return _mapper.Map<RoleDto>(saved);
    }

    private async Task ValidateAsync(Role entity, CancellationToken ct)
    {
        if (await _repository.NameExistsAsync(entity.RoleName, entity.RoleId, ct))
            throw new ConflictException($"Роль '{entity.RoleName}' уже существует.");
    }
}

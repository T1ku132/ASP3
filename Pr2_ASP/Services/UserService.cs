using AutoMapper;
using Pr1_ASP.Dtos;
using Pr1_ASP.Models;
using Pr1_ASP.Repositories;

namespace Pr1_ASP.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IRoleRepository _roles;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository, IRoleRepository roles, IMapper mapper)
    {
        _repository = repository;
        _roles = roles;
        _mapper = mapper;
    }

    public async Task<(List<UserDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var entities = await _repository.GetPageAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<UserDto>>(entities), total);
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct, track: false);
        return entity is null ? null : _mapper.Map<UserDto>(entity);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct)
    {
        var entity = _mapper.Map<User>(dto);
        entity.PasswordHash = PasswordHasher.Hash(dto.Password);
        entity.CreatedAt = DateTime.UtcNow;
        await ValidateAsync(entity, ct);

        await _repository.AddAsync(entity, ct);
        await _repository.SaveChangesAsync(ct);

        return await LoadAsync(entity.UserId, ct);
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        entity.PasswordHash = PasswordHasher.Hash(dto.Password);
        await ValidateAsync(entity, ct);

        _repository.Update(entity);
        await _repository.SaveChangesAsync(ct);

        return await LoadAsync(id, ct);
    }

    // PATCH: профиль маппинга копирует только непустые поля DTO (см. MappingProfile)
    public async Task<UserDto?> PatchAsync(int id, PatchUserDto dto, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        if (dto.Password is not null) entity.PasswordHash = PasswordHasher.Hash(dto.Password);
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
            throw new ConflictException("Нельзя удалить пользователя: есть корзина, заказы или резервирования.");
        _repository.Remove(entity);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    // Перечитываем без отслеживания, чтобы в ответе были актуальные вложенные данные
    private async Task<UserDto> LoadAsync(int id, CancellationToken ct)
    {
        var saved = await _repository.GetByIdAsync(id, ct, track: false)
            ?? throw new InvalidOperationException("Не удалось загрузить сохранённую запись.");
        return _mapper.Map<UserDto>(saved);
    }

    private async Task ValidateAsync(User entity, CancellationToken ct)
    {
        if (!await _roles.ExistsAsync(entity.RoleId, ct))
            throw new InvalidOperationException($"Роль с Id {entity.RoleId} не найдена.");
        if (await _repository.UsernameExistsAsync(entity.Username, entity.UserId, ct))
            throw new ConflictException($"Имя пользователя '{entity.Username}' уже занято.");
        if (await _repository.EmailExistsAsync(entity.Email, entity.UserId, ct))
            throw new ConflictException($"Email '{entity.Email}' уже используется.");
    }
}

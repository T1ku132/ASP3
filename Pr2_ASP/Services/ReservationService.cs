using AutoMapper;
using Pr1_ASP.Dtos;
using Pr1_ASP.Models;
using Pr1_ASP.Repositories;

namespace Pr1_ASP.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repository;
    private readonly IProductRepository _products;
    private readonly IUserRepository _users;
    private readonly IMapper _mapper;

    public ReservationService(
        IReservationRepository repository,
        IProductRepository products,
        IUserRepository users,
        IMapper mapper)
    {
        _repository = repository;
        _products = products;
        _users = users;
        _mapper = mapper;
    }

    public async Task<(List<ReservationDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var items = await _repository.GetPageAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<ReservationDto>>(items), total);
    }

    public async Task<ReservationDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct, track: false);
        return entity is null ? null : _mapper.Map<ReservationDto>(entity);
    }

    // ---- Резервирование: меняет ДВЕ связанные сущности (Product и Reservation) ----
    // Репозитории используют один и тот же Scoped-DbContext, поэтому единственный
    // SaveChangesAsync сохраняет и уменьшенный остаток, и новую запись — атомарно,
    // без UnitOfWork и без явной транзакции (SaveChanges сам выполняется в транзакции).
    public async Task<ReservationDto> ReserveAsync(ReserveProductDto dto, CancellationToken ct)
    {
        if (dto.Quantity <= 0)
            throw new InvalidOperationException("Количество должно быть больше нуля.");

        if (!await _users.ExistsAsync(dto.UserId, ct))
            throw new KeyNotFoundException($"Пользователь с Id {dto.UserId} не найден.");

        var product = await _products.GetByIdAsync(dto.ProductId, ct)
            ?? throw new KeyNotFoundException($"Товар с Id {dto.ProductId} не найден.");

        if (product.IsAvailable == false)
            throw new ConflictException("Товар недоступен для резервирования.");

        if (product.StockQuantity < dto.Quantity)
            throw new ConflictException(
                $"Недостаточно товара на складе: доступно {product.StockQuantity}, запрошено {dto.Quantity}.");

        product.StockQuantity -= dto.Quantity;                 // 1-я сущность: Product

        var reservation = _mapper.Map<Reservation>(dto);       // 2-я сущность: Reservation
        reservation.ReservedAt = DateTime.UtcNow;
        await _repository.AddAsync(reservation, ct);

        await _repository.SaveChangesAsync(ct);                // один вызов на обе сущности

        return await LoadAsync(reservation.ReservationId, ct);
    }

    // ---- Отмена резервирования: обратная операция ----
    public async Task<ReservationDto?> ReleaseAsync(int id, CancellationToken ct)
    {
        // Только активные записи (ReleasedAt == null); Product загружен вместе с ней
        var reservation = await _repository.GetActiveByIdAsync(id, ct);
        if (reservation is null) return null;                  // Controller -> 404

        reservation.Product.StockQuantity += reservation.Quantity;  // вернули остаток
        reservation.ReleasedAt = DateTime.UtcNow;                   // закрыли запись

        await _repository.SaveChangesAsync(ct);                // один вызов на обе сущности

        return await LoadAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var reservation = await _repository.GetByIdAsync(id, ct);
        if (reservation is null) return false;

        if (reservation.ReleasedAt is null)
            throw new ConflictException(
                "Нельзя удалить активное резервирование — сначала отмените его (POST /release), иначе остаток потеряется.");

        _repository.Remove(reservation);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    private async Task<ReservationDto> LoadAsync(int id, CancellationToken ct)
    {
        var saved = await _repository.GetByIdAsync(id, ct, track: false)
            ?? throw new InvalidOperationException("Не удалось загрузить сохранённую запись.");
        return _mapper.Map<ReservationDto>(saved);
    }
}

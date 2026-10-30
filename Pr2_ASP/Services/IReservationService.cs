using Pr1_ASP.Dtos;

namespace Pr1_ASP.Services;

public interface IReservationService
{
    Task<(List<ReservationDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<ReservationDto?> GetByIdAsync(int id, CancellationToken ct);

    // Бизнес-операция: уменьшить остаток товара + создать запись о резервировании
    Task<ReservationDto> ReserveAsync(ReserveProductDto dto, CancellationToken ct);

    // Обратная операция: вернуть остаток + закрыть запись. null — нет АКТИВНОЙ записи.
    Task<ReservationDto?> ReleaseAsync(int id, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

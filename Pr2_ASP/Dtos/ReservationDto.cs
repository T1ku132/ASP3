namespace Pr1_ASP.Dtos;

// Резервирование — для чтения. IsActive = ReleasedAt == null
public class ReservationDto
{
    public int ReservationId { get; set; }
    public UserBriefDto User { get; set; } = null!;
    public ProductBriefDto Product { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime ReservedAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public bool IsActive { get; set; }
}

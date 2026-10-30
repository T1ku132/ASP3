namespace Pr1_ASP.Dtos;

// Корзина — для чтения (с вложенными позициями)
public class CartDto
{
    public int CartId { get; set; }
    public int UserId { get; set; }
    public UserBriefDto User { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdated { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}

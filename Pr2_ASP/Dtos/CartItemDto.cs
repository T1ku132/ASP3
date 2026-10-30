namespace Pr1_ASP.Dtos;

// Позиция корзины — для чтения
public class CartItemDto
{
    public int CartItemId { get; set; }
    public int CartId { get; set; }
    public ProductBriefDto Product { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime? AddedAt { get; set; }
}

namespace Pr1_ASP.Dtos;

// Позиция заказа — для чтения
public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public ProductBriefDto Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

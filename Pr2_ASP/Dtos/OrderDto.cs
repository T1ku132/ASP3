namespace Pr1_ASP.Dtos;

// Заказ — для чтения (с вложенными позициями)
public class OrderDto
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public UserBriefDto User { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? ShippingAddress { get; set; }
    public string? PaymentMethod { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

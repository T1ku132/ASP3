namespace Pr1_ASP.Dtos;

// Краткий товар — вкладывается в CartItem/OrderItem/Reservation
public class ProductBriefDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public decimal Price { get; set; }
}

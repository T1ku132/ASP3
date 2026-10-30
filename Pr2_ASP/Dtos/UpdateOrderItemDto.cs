using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// PUT — все поля обязательны
public class UpdateOrderItemDto
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, 100000000)]
    public decimal UnitPrice { get; set; }
}

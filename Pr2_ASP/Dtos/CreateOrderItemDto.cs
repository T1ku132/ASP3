using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// для записи (POST): Id связанных сущностей, без вложенных объектов
public class CreateOrderItemDto
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, 100000000)]
    public decimal UnitPrice { get; set; }
}

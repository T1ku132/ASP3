using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// для записи (POST): Id связанных сущностей, без вложенных объектов
public class CreateOrderDto
{
    public int UserId { get; set; }

    [Range(0, 100000000)]
    public decimal TotalAmount { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [StringLength(500)]
    public string? ShippingAddress { get; set; }

    [StringLength(50)]
    public string? PaymentMethod { get; set; }
}

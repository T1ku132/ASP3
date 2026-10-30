using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// PUT — все поля обязательны
public class UpdateCartItemDto
{
    public int CartId { get; set; }

    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

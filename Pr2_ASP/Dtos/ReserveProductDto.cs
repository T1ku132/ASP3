using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// Запрос на резервирование товара под пользователя
public class ReserveProductDto
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

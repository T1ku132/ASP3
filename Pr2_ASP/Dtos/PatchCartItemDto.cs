using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// PATCH — все поля nullable, меняются только переданные
public class PatchCartItemDto
{
    public int? CartId { get; set; }

    public int? ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int? Quantity { get; set; }
}

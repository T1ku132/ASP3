using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// PATCH — все поля nullable, меняются только переданные
public class PatchCategoryDto
{
    [StringLength(100)]
    public string? CategoryName { get; set; }

    public int? ParentCategoryId { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// PUT — все поля обязательны
public class UpdateCategoryDto
{
    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = "";

    public int? ParentCategoryId { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }
}

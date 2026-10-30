using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// для записи (POST): Id связанных сущностей, без вложенных объектов
public class CreateCategoryDto
{
    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = "";

    public int? ParentCategoryId { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }
}

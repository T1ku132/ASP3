using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// для записи (POST): Id связанных сущностей, без вложенных объектов
public class CreateProductDto
{
    [Required]
    [StringLength(100)]
    public string ProductName { get; set; } = "";

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(0, 100000000)]
    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    public bool? IsAvailable { get; set; } = true;

    [StringLength(255)]
    public string? ImagePath { get; set; }

    [StringLength(255)]
    public string? ImageFileName { get; set; }
}

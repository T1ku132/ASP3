namespace Pr1_ASP.Dtos;

// Товар — для чтения
public class ProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool? IsAvailable { get; set; }
    public string? ImagePath { get; set; }
    public string? ImageFileName { get; set; }
    public CategoryBriefDto Category { get; set; } = null!;
}

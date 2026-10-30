namespace Pr1_ASP.Dtos;

// Категория — для чтения
public class CategoryDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
}

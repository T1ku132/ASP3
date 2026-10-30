using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// для записи (POST): Id связанных сущностей, без вложенных объектов
public class CreateRoleDto
{
    [Required]
    [StringLength(50)]
    public string RoleName { get; set; } = "";

    [StringLength(255)]
    public string? Description { get; set; }
}

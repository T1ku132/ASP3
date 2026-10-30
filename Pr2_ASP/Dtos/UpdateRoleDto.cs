using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// PUT — все поля обязательны
public class UpdateRoleDto
{
    [Required]
    [StringLength(50)]
    public string RoleName { get; set; } = "";

    [StringLength(255)]
    public string? Description { get; set; }
}

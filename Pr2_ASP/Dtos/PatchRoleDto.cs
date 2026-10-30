using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// PATCH — все поля nullable, меняются только переданные
public class PatchRoleDto
{
    [StringLength(50)]
    public string? RoleName { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }
}

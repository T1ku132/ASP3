using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// PATCH — все поля nullable, меняются только переданные
public class PatchUserDto
{
    [StringLength(50)]
    public string? Username { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(100, MinimumLength = 6)]
    public string? Password { get; set; }

    public int? RoleId { get; set; }

    public bool? IsActive { get; set; }
}

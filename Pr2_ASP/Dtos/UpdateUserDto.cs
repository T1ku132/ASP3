using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// PUT — все поля обязательны
public class UpdateUserDto
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = "";

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = "";

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = "";

    public int RoleId { get; set; } = 2;

    public bool? IsActive { get; set; } = true;
}

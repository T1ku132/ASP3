using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// для записи (POST): Id связанных сущностей, без вложенных объектов
public class CreateUserDto
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

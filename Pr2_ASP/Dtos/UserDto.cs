namespace Pr1_ASP.Dtos;

// Пользователь — для чтения (PasswordHash намеренно не отдаётся)
public class UserDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime? CreatedAt { get; set; }
    public bool? IsActive { get; set; }
    public RoleDto Role { get; set; } = null!;
}

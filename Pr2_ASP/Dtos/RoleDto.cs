namespace Pr1_ASP.Dtos;

// Роль — для чтения
public class RoleDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = "";
    public string? Description { get; set; }
}

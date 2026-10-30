using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// PUT — все поля обязательны
public class UpdateCartDto
{
    public int UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastUpdated { get; set; }
}

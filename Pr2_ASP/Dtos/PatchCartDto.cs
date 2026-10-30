using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// PATCH — все поля nullable, меняются только переданные
public class PatchCartDto
{
    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastUpdated { get; set; }
}

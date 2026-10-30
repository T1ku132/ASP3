using System.ComponentModel.DataAnnotations;

namespace Pr1_ASP.Dtos;

// для записи (POST): Id связанных сущностей, без вложенных объектов
public class CreateCartDto
{
    public int UserId { get; set; }
}

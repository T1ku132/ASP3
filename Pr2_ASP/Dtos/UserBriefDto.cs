namespace Pr1_ASP.Dtos;

// Краткий пользователь — вкладывается в Cart/Order/Reservation
public class UserBriefDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = "";
}

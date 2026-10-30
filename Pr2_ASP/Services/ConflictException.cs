namespace Pr1_ASP.Services;

// Бизнес-условие не выполнено (недостаточно остатка, дубликат, есть зависимые записи).
// Controller переводит это в HTTP 409.
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}

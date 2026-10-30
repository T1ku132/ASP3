using System.Security.Cryptography;
using System.Text;

namespace Pr1_ASP.Services;

// Колонка password_hash — varchar(64): SHA-256 в hex ровно столько и занимает.
// Для учебного проекта достаточно; в реальном — использовать PBKDF2/bcrypt/Argon2 с солью.
public static class PasswordHasher
{
    public static string Hash(string password) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password))).ToLowerInvariant();
}

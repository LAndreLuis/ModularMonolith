using Microsoft.AspNetCore.Identity;

namespace Modules.Identity.Services
{
    public static class PasswordHasherService
    {
        private static readonly PasswordHasher<string> Hasher = new();

        public static string HashPassword(string password) =>
            Hasher.HashPassword("user", password);

        public static bool VerifyPassword(string hashedPassword, string providedPassword) =>
            Hasher.VerifyHashedPassword("user", hashedPassword, providedPassword) != PasswordVerificationResult.Failed;
    }
}
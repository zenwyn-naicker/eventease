namespace TheEventEaseWebApp
{
    using BCrypt.Net;
    public class PasswordService
    {
        public static string HashPassword(string password)
        {
            return BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Verify(password, hash);
        }

    }
}

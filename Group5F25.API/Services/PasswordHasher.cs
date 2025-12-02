using System.Security.Cryptography;
using System.Text;

namespace Group5F25.API.Services
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var pwd = Encoding.UTF8.GetBytes(password);
            var toHash = new byte[salt.Length + pwd.Length];
            Buffer.BlockCopy(salt, 0, toHash, 0, salt.Length);
            Buffer.BlockCopy(pwd, 0, toHash, salt.Length, pwd.Length);
            var hash = SHA256.HashData(toHash);
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public static bool VerifyPassword(string password, string stored)
        {
            var parts = stored.Split(':');
            if (parts.Length != 2) return false;
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]);

            var pwd = Encoding.UTF8.GetBytes(password);
            var toHash = new byte[salt.Length + pwd.Length];
            Buffer.BlockCopy(salt, 0, toHash, 0, salt.Length);
            Buffer.BlockCopy(pwd, 0, toHash, salt.Length, pwd.Length);
            var hash = SHA256.HashData(toHash);

            if (hash.Length != storedHash.Length) return false;
            for (int i = 0; i < hash.Length; i++)
                if (hash[i] != storedHash[i]) return false;
            return true;
        }
    }
}

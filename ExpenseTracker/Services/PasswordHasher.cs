using System;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseTracker.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
        }

        public bool ValidatePassword(string enteredPassword, string storedHash, string salt)
        {
            string enteredPasswordHash = HashPassword(enteredPassword, salt);
            return enteredPasswordHash == storedHash;
        }

        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var cryptoService = new RNGCryptoServiceProvider())
            {
                cryptoService.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
    }
}


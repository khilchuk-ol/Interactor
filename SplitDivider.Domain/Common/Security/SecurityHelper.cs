using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SplitDivider.Domain.Common.Security;

public class SecurityHelper
{
    public static string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }

        public static byte[] GetRandomBytes(int length = 32)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var salt = new byte[length];
                randomNumberGenerator.GetBytes(salt);

                return salt;
            }
        }

        public static bool IsValidPassword(string hasedPassword, string password, string salt)
        {
            var hash = HashPassword(password, Convert.FromBase64String(salt));

            return hasedPassword == hash;
        }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.Helpers
{
    public static class PasswordHasher
    {
        private const int SaltSize = 32;
        private const int HashSize = 64;
        private const int Iterations = 100000;
        public static void HashPassword(
            string password,
            out byte[] passwordHash,
            out byte[] passwordSalt)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                passwordSalt = new byte[SaltSize];
                rng.GetBytes(passwordSalt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                passwordSalt,
                Iterations))
            {
                passwordHash = pbkdf2.GetBytes(HashSize);
            }
        }
        public static bool VerifyPassword(
            string password,
            byte[] storedHash,
            byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                storedSalt,
                Iterations))
            {
                byte[] computedHash = pbkdf2.GetBytes(HashSize);

                return FixedTimeEquals(computedHash, storedHash);
            }
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int difference = 0;

            for (int i = 0; i < a.Length; i++)
            {
                difference |= a[i] ^ b[i];
            }
            return difference == 0;
        }
    }
}

using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Domain.Helper
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            var salt = new byte[16];
            RandomNumberGenerator.Fill(salt);

            var argon2 = new Argon2i(Encoding.UTF8.GetBytes(password));
            argon2.Salt = salt;
            argon2.Iterations = 4;
            argon2.MemorySize = 128 * 1024;
            argon2.DegreeOfParallelism = 1;

            var hashedPassword = argon2.GetBytes(16);

            var stringedHashedPassword = Convert.ToBase64String(hashedPassword);
            var stringedSalt = Convert.ToBase64String(salt);
            return stringedSalt + stringedHashedPassword;
        }

        public static bool CheckPassword(string password, string inputPassword)
        {
            var argon2 = new Argon2i(Encoding.UTF8.GetBytes(inputPassword));
            argon2.Iterations = 4;
            argon2.MemorySize = 128 * 1024;
            argon2.DegreeOfParallelism = 1;

            var salt = password.Split("==")[0] + "==";
            argon2.Salt = Convert.FromBase64String(salt);

            var hashedPassword = argon2.GetBytes(16);

            return salt + Convert.ToBase64String(hashedPassword) == password;
        }
    }
}
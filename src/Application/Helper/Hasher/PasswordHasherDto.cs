using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper.Hasher
{
    public class PasswordHasherDto
    {
        public static string Hasher(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                var sha = SHA256.Create();
                var asByteArray = Encoding.UTF8.GetBytes(password);

                var hasherPassword = sha.ComputeHash(asByteArray);

                return Convert.ToBase64String(hasherPassword);
            }
            return null;
        }
    }
}

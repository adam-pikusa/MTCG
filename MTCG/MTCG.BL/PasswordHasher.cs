using System.Security.Cryptography;
using System.Text;

namespace MTCG.BL
{
    public class PasswordHasher
    {
        SHA256 sha = SHA256.Create();

        public string Hash(string password)
        {
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            foreach (byte b in hash) sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
    }
}

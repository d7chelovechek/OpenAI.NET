using System.Security.Cryptography;
using System.Text;

namespace OpenAI.NET.Web.Services
{
    public class Sha256
    {
        public static string GetHash(string value)
        {
            using (SHA256 algorithm = SHA256.Create())
            {
                return Encoding.UTF8.GetString(algorithm.ComputeHash(Encoding.ASCII.GetBytes(value)));
            }
        }
    }
}
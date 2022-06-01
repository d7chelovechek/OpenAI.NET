using System.Linq;
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
                string result = string.Empty;

                algorithm.ComputeHash(Encoding.ASCII.GetBytes(value))
                    .ToList()
                    .ForEach(x => result += x.ToString("x2"));

                return result;
            }
        }
    }
}
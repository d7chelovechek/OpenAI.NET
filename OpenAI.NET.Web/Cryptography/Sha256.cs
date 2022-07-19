using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OpenAI.NET.Web.Cryptography
{
    /// <summary>
    /// Sha256 generator.
    /// </summary>
    public class Sha256
    {
        /// <summary>
        /// Creating a hash from a string.
        /// </summary>
        /// <returns>Sha256 hash</returns>
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
using OpenAI.NET.Models.Jwt.Auth;
using System.Threading.Tasks;

namespace OpenAI.NET.Lib.Controllers.Interfaces
{
    /// <summary>
    /// Description of Jwt controller.
    /// </summary>
    public interface IJwt 
    {
        /// <summary>
        /// Async authorization in OpenAI.NET.Web.
        /// </summary>
        /// <returns>AccessToken</returns>
        public Task<string> AuthAsync(AuthRequestParameters parameters);
        /// <summary>
        /// Authorization in OpenAI.NET.Web.
        /// </summary>
        /// <returns>AccessToken</returns>
        public string Auth(AuthRequestParameters parameters);
        /// <summary>
        /// Authorization in OpenAI.NET.Web by AccessToken.
        /// </summary>
        public void Auth(string accessToken);
    }
}
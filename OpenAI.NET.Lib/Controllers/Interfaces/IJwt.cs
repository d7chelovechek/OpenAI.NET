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
        /// <returns>AuthResponse body.</returns>
        public Task<AuthResponse> AuthAsync(AuthRequest request);
        /// <summary>
        /// Authorization in OpenAI.NET.Web.
        /// </summary>
        /// <returns>AuthResponse body.</returns>
        public AuthResponse Auth(AuthRequest request);
        /// <summary>
        /// Authorization in OpenAI.NET.Web by access token.
        /// </summary>
        public void Auth(string accessToken);
    }
}
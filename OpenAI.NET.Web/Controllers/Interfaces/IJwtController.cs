using Microsoft.AspNetCore.Mvc;
using OpenAI.NET.Models.Jwt.Add;
using OpenAI.NET.Models.Jwt.Auth;
using OpenAI.NET.Models.Jwt.Remove;
using System.Threading.Tasks;

namespace OpenAI.NET.Web.Controllers.Interfaces
{
    /// <summary>
    /// Description of Api controller.
    /// </summary>
    public interface IJwtController
    {
        /// <summary>
        /// Async authorization in OpenAI.NET.Web.
        /// </summary>
        /// <returns>Json content.</returns>
        public Task<IActionResult> AuthAsync(
            AuthRequestParameters request);
        /// <summary>
        /// Async adding new user in OpenAI.NET.Web.
        /// </summary>
        /// <returns>Json content.</returns>
        public Task<IActionResult> AddAsync(
            AddRequestParameters request);
        /// <summary>
        /// Async removing user from OpenAI.NET.Web.
        /// </summary>
        /// <returns>Json content.</returns>
        public Task<IActionResult> RemoveAsync(
            RemoveRequestParameters request);
    }
}
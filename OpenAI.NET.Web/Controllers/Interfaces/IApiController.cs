using Microsoft.AspNetCore.Mvc;
using OpenAI.NET.Models.Api.Complete;
using System.Threading.Tasks;

namespace OpenAI.NET.Web.Controllers.Interfaces
{
    /// <summary>
    /// Description of Api controller.
    /// </summary>
    public interface IApiController
    {
        /// <summary>
        /// Async request to continue text by OpenAI.Api.
        /// </summary>
        /// <returns>Json content.</returns>
        public Task<IActionResult> CompleteAsync(
            CompleteRequestParameters request);
    }
}
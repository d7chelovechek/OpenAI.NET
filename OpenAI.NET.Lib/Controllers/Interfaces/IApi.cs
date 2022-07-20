using OpenAI.NET.Models.Api.Complete;
using System.Threading.Tasks;

namespace OpenAI.NET.Lib.Controllers.Interfaces
{
    /// <summary>
    /// Description of Api controller.
    /// </summary>
    public interface IApi
    {
        /// <summary>
        /// Async completing text by OpenAI.NET.Web.
        /// </summary>
        /// <returns>CompleteResponse body.</returns>
        public Task<CompleteResponse> CompleteAsync(
            CompleteRequest request);
        /// <summary>
        /// Completing text by OpenAI.NET.Web.
        /// </summary>
        /// <returns>CompleteResponse body.</returns>
        public CompleteResponse Complete(
            CompleteRequest request);
    }
}
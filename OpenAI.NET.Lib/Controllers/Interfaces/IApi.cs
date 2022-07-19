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
        /// <returns>Completion</returns>
        public Task<string> CompleteAsync(
            CompleteRequestParameters parameters);
    }
}
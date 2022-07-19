using OpenAI.NET.Lib.Controllers.Interfaces;
using OpenAI.NET.Models.Api.Complete;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenAI.NET.Lib.Controllers
{
    /// <summary>
    /// Api controller.
    /// </summary>
    public class Api : BaseController, IApi
    {
        private readonly Client _client;

        /// <summary>
        /// A constructor that initializes all fields.
        /// </summary>
        public Api(Client client)
        {
            _client = client;
        }
        
        public async Task<string> CompleteAsync(
            CompleteRequestParameters request)
        {
            HttpResponseMessage responseMessage =
                await _client.HttpClient.PostAsync(
                    "api/complete",
                    new FormUrlEncodedContent(
                        GetContent(request)));

            object result =
                await DeserializeResponseAsync<CompleteResponseBody>(
                    responseMessage,
                    GetCompletion);

            return result.ToString();
        }

        /// <summary>
        /// Method that executing in
        /// <see cref="BaseController.DeserializeResponseAsync{T}(HttpResponseMessage, Action{T})"/>.
        /// </summary>
        /// <returns>Completion</returns>
        private string GetCompletion(CompleteResponseBody response)
        {
            return response.Completion;
        }
    }
}
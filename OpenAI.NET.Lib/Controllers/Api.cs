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
        
        public async Task<CompleteResponse> CompleteAsync(
            CompleteRequest request)
        {
            HttpResponseMessage responseMessage =
                await _client.HttpClient.PostAsync(
                    "api/complete",
                    new FormUrlEncodedContent(
                        GetContent(request)));

            CompleteResponse response =
                await DeserializeResponseAsync<CompleteResponse>(
                    responseMessage);

            return response;
        }

        public CompleteResponse Complete(
            CompleteRequest request)
        {
            return CompleteAsync(request).GetAwaiter().GetResult();
        }
    }
}
using Newtonsoft.Json;
using OpenAI.NET.Lib.Services;
using OpenAI.NET.Models;
using OpenAI.NET.Models.Api.Complete;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenAI.NET.Lib
{
    public class Api
    {
        private readonly HttpClient _client;

        public Api(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> CompleteAsync(CompleteRequestParameters parameters)
        {
            HttpResponseMessage responseMessage = await _client.PostAsync(
                "https://openai-net.herokuapp.com/api/complete",
                new FormUrlEncodedContent(LibHelper.GetContent(
                    typeof(CompleteRequestParameters),
                    parameters)));

            Response response =
                JsonConvert.DeserializeObject<Response>(await responseMessage.Content.ReadAsStringAsync());

            if (responseMessage.IsSuccessStatusCode)
            {
                if (response.Exceptions is null)
                {
                    CompleteResponseBody body =
                        JsonConvert.DeserializeObject<CompleteResponseBody>(response.Body.ToString());

                    return body.Completion;
                }
                else
                {
                    throw LibHelper.GetException(response.Body.ToString(), response.Exceptions);
                }
            }
            else
            {
                throw LibHelper.GetException(responseMessage.ReasonPhrase, null);
            }
        }
    }
}
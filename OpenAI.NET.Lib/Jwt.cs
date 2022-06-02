using Newtonsoft.Json;
using OpenAI.NET.Lib.Services;
using OpenAI.NET.Models;
using OpenAI.NET.Models.Jwt.Auth;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenAI.NET.Lib
{
    public class Jwt
    {
        private readonly HttpClient _client;

        public Jwt(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> AuthAsync(AuthRequestParameters parameters)
        {
            HttpResponseMessage responseMessage = await _client.PostAsync(
                "https://openai-net.herokuapp.com/jwt/auth",
                new FormUrlEncodedContent(LibHelper.GetContent(
                    typeof(AuthRequestParameters),
                    parameters)));

            Response response =
                JsonConvert.DeserializeObject<Response>(await responseMessage.Content.ReadAsStringAsync());

            if (response.Exceptions is null)
            {
                AuthResponseBody body =
                    JsonConvert.DeserializeObject<AuthResponseBody>(response.Body.ToString());

                _client.DefaultRequestHeaders.Authorization = null;
                _client.DefaultRequestHeaders
                    .Add("Authorization", $"Bearer {body.AccessToken}");

                return body.AccessToken;
            }

            throw LibHelper.GetException(response.Body.ToString(), response.Exceptions);
        }

        public string Auth(string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = null;
            _client.DefaultRequestHeaders
                .Add("Authorization", $"Bearer {accessToken}");

            return accessToken;
        }
    }
}
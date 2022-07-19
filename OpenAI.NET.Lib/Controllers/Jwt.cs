using OpenAI.NET.Lib.Controllers.Interfaces;
using OpenAI.NET.Models.Jwt.Auth;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenAI.NET.Lib.Controllers
{
    /// <summary>
    /// Jwt controller.
    /// </summary>
    public class Jwt : BaseController, IJwt
    {
        private readonly Client _client;

        /// <summary>
        /// A constructor that initializes all fields.
        /// </summary>
        public Jwt(Client client)
        {
            _client = client;
        }

        public async Task<string> AuthAsync(
            AuthRequestParameters request)
        {
            HttpResponseMessage responseMessage =
                await _client.HttpClient.PostAsync(
                    "jwt/auth",
                    new FormUrlEncodedContent(
                        GetContent(request)));

            object result =
                await DeserializeResponseAsync<AuthResponseBody>(
                    responseMessage,
                    GetAccessToken);

            return result.ToString();
        }

        /// <summary>
        /// Method that executing in
        /// <see cref="BaseController.DeserializeResponseAsync{T}(HttpResponseMessage, Action{T})"/>.
        /// </summary>
        /// <returns>AccessToken</returns>
        private string GetAccessToken(AuthResponseBody response)
        {
            _client.AddAuthorizationHeader(response.AccessToken);

            return response.AccessToken;
        }

        public string Auth(AuthRequestParameters request)
        {
            return AuthAsync(request).GetAwaiter().GetResult();
        }

        public void Auth(string accessToken)
        {
            _client.AddAuthorizationHeader(accessToken);
        }
    }
}
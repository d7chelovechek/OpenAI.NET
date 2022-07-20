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

        public async Task<AuthResponse> AuthAsync(
            AuthRequest request)
        {
            HttpResponseMessage responseMessage =
                await _client.HttpClient.PostAsync(
                    "jwt/auth",
                    new FormUrlEncodedContent(
                        GetContent(request)));

            AuthResponse response =
                await DeserializeResponseAsync<AuthResponse>(
                    responseMessage);

            _client.AddAuthorizationHeader(response.AccessToken);

            return response;
        }

        public AuthResponse Auth(AuthRequest request)
        {
            return AuthAsync(request).GetAwaiter().GetResult();
        }

        public void Auth(string accessToken)
        {
            _client.AddAuthorizationHeader(accessToken);
        }
    }
}
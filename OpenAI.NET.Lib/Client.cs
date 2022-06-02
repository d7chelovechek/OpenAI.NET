using System.Net.Http;

namespace OpenAI.NET.Lib
{
    public class Client
    {
        public Jwt Jwt { get; private set; }
        public Api Api { get; private set; }

        private readonly HttpClient _client;

        public Client()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders
                .Add("User-Agent", "OpenAI.NET-Client");

            Jwt = new Jwt(_client);
            Api = new Api(_client);
        }
    }
}
using OpenAI.NET.Lib.Controllers;
using OpenAI.NET.Lib.Controllers.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OpenAI.NET.Lib
{
    /// <summary>
    /// Client to work with OpenAI.NET.Web. Acts as a factory.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// HttpClient for sending requests to OpenAI.NET.Web.
        /// </summary>
        internal HttpClient HttpClient { get; private set; }

        /// <summary>
        /// Address where OpenAI.NET.Web is deployed.
        /// </summary>
        public string Address
        {
            get => _address;
            set
            {
                _address = value.EndsWith('/') ?
                    value : value + "/";
                HttpClient.BaseAddress =
                    new Uri(_address);
            }
        }
        private string _address;

        /// <summary>
        /// Token for authorization in OpenAI.NET.Web.
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Jwt controller.
        /// </summary>
        public IJwt Jwt { get; private set; }
        /// <summary>
        /// Api controller.
        /// </summary>
        public IApi Api { get; private set; }

        /// <summary>
        /// A constructor that initializes all factory properties.
        /// </summary>
        public Client(string host)
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders
                .Add("User-Agent", "OpenAI.NET-Client");

            Address = host;

            Jwt = new Jwt(this);
            Api = new Api(this);
        }

        /// <summary>
        /// A constructor that initializes all factory properties and
        /// logs in OpenAI.NET.Web.
        /// </summary>
        public Client(string host, string accessToken) : this(host)
        {
            AddAuthorizationHeader(accessToken);
        }

        /// <summary>
        /// Storing <see cref="AccessToken"/> and
        /// adding it to <see cref="HttpClient"/> header.
        /// </summary>
        internal void AddAuthorizationHeader(string accessToken)
        {
            AccessToken = accessToken;

            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
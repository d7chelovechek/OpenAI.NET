﻿namespace OpenAI.NET.Web.Models.Jwt.Auth
{
    public class AuthResponseBody
    {
        public string Name { get; set; }
        public string Permission { get; set; }
        public string AccessToken { get; set; }
        public string ExpirationDate { get; set; }
    }
}
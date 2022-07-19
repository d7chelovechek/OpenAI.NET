namespace OpenAI.NET.Models.Jwt.Auth
{
    /// <summary>
    /// Body received after calling Auth action.
    /// </summary>
    public class AuthResponseBody
    {
        /// <summary>
        /// User name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Access rights to OpenAI.NET.
        /// </summary>
        public string[] Permissions { get; set; }
        /// <summary>
        /// Access token to OpenAI.NET.
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Token expiration date.
        /// </summary>
        public string ExpirationDate { get; set; }
    }
}
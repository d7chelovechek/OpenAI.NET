namespace OpenAI.NET.Models.Jwt.Auth
{
    /// <summary>
    /// Parameters for sending a request to Auth action.
    /// </summary>
    public class AuthRequestParameters
    {
        /// <summary>
        /// User name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Un-hashed password.
        /// </summary>
        public string Password { get; set; }
    }
}
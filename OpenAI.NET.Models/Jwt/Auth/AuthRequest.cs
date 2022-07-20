namespace OpenAI.NET.Models.Jwt.Auth
{
    /// <summary>
    /// Auth action request body.
    /// </summary>
    public class AuthRequest
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
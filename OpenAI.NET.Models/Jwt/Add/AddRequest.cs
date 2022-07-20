namespace OpenAI.NET.Models.Jwt.Add
{
    /// <summary>
    /// Add action request body.
    /// </summary>
    public class AddRequest
    {
        /// <summary>
        /// User name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Un-hashed password.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Access rights to OpenAI.NET.
        /// </summary>
        public string[] Permissions { get; set; }
        /// <summary>
        /// Lifetime of authorization token.
        /// </summary>
        public string TokenLifeTime { get; set; }
    }
}
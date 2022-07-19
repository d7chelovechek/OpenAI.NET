namespace OpenAI.NET.Models.Jwt.Add
{
    /// <summary>
    /// Parameters for sending a request to Add action.
    /// </summary>
    public class AddRequestParameters
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
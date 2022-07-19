using System;

namespace OpenAI.NET.Models.Jwt.Remove
{
    /// <summary>
    /// Body received after calling Remove action.
    /// </summary>
    public class RemoveResponseBody
    {
        /// <summary>
        /// User name.
        /// </summary>
        public string Name { get; set; }
    }
}
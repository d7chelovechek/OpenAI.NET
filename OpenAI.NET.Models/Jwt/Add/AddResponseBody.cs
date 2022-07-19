﻿namespace OpenAI.NET.Models.Jwt.Add
{
    /// <summary>
    /// Body received after calling Add action.
    /// </summary>
    public class AddResponseBody
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
        /// Lifetime of authorization token.
        /// </summary>
        public string TokenLifeTime { get; set; }
    }
}
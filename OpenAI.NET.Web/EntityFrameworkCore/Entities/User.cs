using System;

namespace OpenAI.NET.Web.EntityFrameworkCore.Entities
{
    /// <summary>
    /// OpenAI.NET.Web user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Hashed password.
        /// </summary>
        public string PasswordHash { get; set; }
        /// <summary>
        /// Access rights to OpenAI.NET.
        /// </summary>
        public string[] Permissions { get; set; }
        /// <summary>
        /// Lifetime of authorization token.
        /// </summary>
        public TimeSpan TokenLifeTime { get; set; }
    }
}
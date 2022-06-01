using System;

namespace OpenAI.NET.Web.EntityFrameworkCore.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string[] Permissions { get; set; }
        public TimeSpan TokenLifeTime { get; set; }
    }
}
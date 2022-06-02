using System;

namespace OpenAI.NET.Models.Jwt.Add
{
    public class AddResponseBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string[] Permissions { get; set; }
        public string TokenLifeTime { get; set; }
    }
}
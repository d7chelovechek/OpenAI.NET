using System;

namespace OpenAI.NET.Web.Models.Jwt.Add
{
    public class AddResponseBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string[] Permissions { get; set; }
        public string TokenLifeTime { get; set; }
    }
}
using System;

namespace OpenAI.NET.Web.Models.Jwt.Remove
{
    public class RemoveResponseBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
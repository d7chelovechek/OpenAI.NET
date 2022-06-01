using System.Collections.Generic;

namespace OpenAI.NET.Models
{
    public class Response
    {
        public string Message { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public List<string> Errors { get; set; }
    }
}
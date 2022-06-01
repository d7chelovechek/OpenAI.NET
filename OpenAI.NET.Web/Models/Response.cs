using System.Collections.Generic;

namespace OpenAI.NET.Web.Models
{
    public class Response
    {
        public object Body { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public List<string> Errors { get; set; }
    }
}
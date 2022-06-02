namespace OpenAI.NET.Models
{
    public class Response
    {
        public object Body { get; set; }
        public List<Parameter> Parameters { get; set; }
        public List<string> Exceptions { get; set; }
    }
}
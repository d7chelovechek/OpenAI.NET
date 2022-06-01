namespace OpenAI.NET.Web.Models.Jwt.Add
{
    public class AddRequestParameters
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string[] Permissions { get; set; }
        public string TokenLifeTime { get; set; }
    }
}
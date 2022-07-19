namespace OpenAI.NET.Models.Response
{
    /// <summary>
    /// Parameter sent by request.
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Parameter name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Parameter value.
        /// </summary>
        public string Value { get; set; }
    }
}
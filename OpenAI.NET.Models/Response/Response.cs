using System.Collections.Generic;

namespace OpenAI.NET.Models.Response
{
    /// <summary>
    /// Request response body.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Main body.
        /// </summary>
        public object Body { get; set; }
        /// <summary>
        /// Parameters sent by request.
        /// </summary>
        public List<Parameter> Parameters { get; set; }
        /// <summary>
        /// Exceptions caused during request processing.
        /// </summary>
        public List<string> Exceptions { get; set; }
    }
}
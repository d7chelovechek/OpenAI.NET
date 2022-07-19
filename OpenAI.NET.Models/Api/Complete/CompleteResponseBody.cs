namespace OpenAI.NET.Models.Api.Complete
{
    /// <summary>
    /// Body received after calling Complete action.
    /// </summary>
    public class CompleteResponseBody
    {
        /// <summary>
        /// Сontinued text.
        /// </summary>
        public string Completion { get; set; }
    }
}
using System.Threading.Tasks;
using OpenAI.NET.Models.Api.Complete;

namespace OpenAI.NET.Web.Translators.Interfaces
{
    /// <summary>
    /// Description of translator.
    /// </summary>
    public interface ITranslator
    {
        /// <summary>
        /// Async translation of text into English.
        /// </summary>
        /// <returns>English text</returns>
        public Task<string> TranslateIntoEnglishAsync(string text, string language);
        /// <summary>
        /// Async translation of text from English.
        /// </summary>
        /// <returns>Text in <see cref="CompleteRequest.ResponseLanguage"/> language</returns>
        public Task<string> TranslateFromEnglishAsync(string text, string language);
    }
}
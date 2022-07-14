using System.Threading.Tasks;

namespace OpenAI.NET.Web.Translators.Interfaces
{
    public interface ITranslator
    {
        public Task<string> TranslateIntoEnglish(string text, string language);
        public Task<string> TranslateFromEnglish(string text, string language);
    }
}
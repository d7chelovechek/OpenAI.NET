using OpenAI.NET.Web.Translators.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.NET.Web.Translators
{
    public class GoogleTranslator : ITranslator
    {
        private const string _mainLanguage = "en";

        private readonly HttpClient _client;

        public GoogleTranslator()
        {
            _client = new HttpClient();
        }

        public async Task<string> TranslateFromEnglish(string text, string language)
        {
            try
            {
                if (language is not _mainLanguage)
                {
                    string response = await _client.GetStringAsync(
                        $"https://translate.googleapis.com/translate_a/single?" +
                        $"client=gtx&sl={_mainLanguage}&tl={language}&dt=t&q={Uri.EscapeDataString(text)}");

                    response = response.Replace($"\"{_mainLanguage}\",", "");

                    return JsonConvert.DeserializeObject<List<List<List<dynamic>>>>(
                        response)[0][0][0].ToString();
                }
                else
                {
                    return text;
                }
            }
            catch
            {
                throw new Exception("Failed to translate text from English");
            }
        }

        public async Task<string> TranslateIntoEnglish(string text, string language)
        {
            try
            {
                if (language is not _mainLanguage)
                {
                    string response = await _client.GetStringAsync(
                        $"https://translate.googleapis.com/translate_a/single?" +
                        $"client=gtx&sl={language}&tl={_mainLanguage}&dt=t&q={Uri.EscapeDataString(text)}");

                    response = response.Replace($"\"{language}\",", "");

                    return JsonConvert.DeserializeObject<List<List<List<dynamic>>>>(
                        response)[0][0][0].ToString();
                }
                else
                {
                    return text;
                }
            }
            catch
            {
                throw new Exception("Failed to translate text into English");
            }
        }
    }
}
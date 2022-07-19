using OpenAI.NET.Web.Translators.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace OpenAI.NET.Web.Translators
{
    /// <summary>
    /// Google translate implementation.
    /// </summary>
    public class GoogleTranslator : ITranslator
    {
        private const string _mainLanguage = "en";

        private readonly HttpClient _client;

        /// <summary>
        /// A constructor that initializes all fields.
        /// </summary>
        public GoogleTranslator(
            IConfiguration configuration)
        {
            string host = configuration["TranslatorsUri:Google"];
            _client = new HttpClient()
            {
                BaseAddress =
                    new Uri(host.EndsWith('/') ? host : host + "/"),
            };
        }

        public async Task<string> TranslateFromEnglishAsync(
            string text,
            string language)
        {
            try
            {
                if (language is not _mainLanguage)
                {
                    text = await TranslateAsync(
                        text,
                        _mainLanguage,
                        language);
                }

                return text;
            }
            catch
            {
                throw new Exception(
                    "Failed to translate text from English");
            }
        }

        public async Task<string> TranslateIntoEnglishAsync(
            string text,
            string language)
        {
            try
            {
                if (language is not _mainLanguage)
                {
                    text = await TranslateAsync(
                        text,
                        language,
                        _mainLanguage);
                }

                return text;
            }
            catch
            {
                throw new Exception(
                    "Failed to translate text into English");
            }
        }

        /// <summary>
        /// Async sending a request to a translator.
        /// </summary>
        /// <returns>Translated text</returns>
        private async Task<string> TranslateAsync(
            string text,
            string fromLanguage,
            string intoLanguage)
        {
            string response = await _client.GetStringAsync(
                $"single?client=gtx&sl={fromLanguage}&tl={intoLanguage}&" +
                $"dt=t&q={Uri.EscapeDataString(text)}");

            response = response.Replace($"\"{fromLanguage}\",", "");

            return JsonConvert.DeserializeObject<List<List<List<dynamic>>>>(
                response)[0][0][0].ToString();
        }
    }
}
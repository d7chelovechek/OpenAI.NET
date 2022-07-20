using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI.NET.Models.Api.Complete;
using OpenAI.NET.Models.Response;
using OpenAI.NET.Models.Web;
using OpenAI.NET.Web.Controllers.Interfaces;
using OpenAI.NET.Web.Translators;
using OpenAI.NET.Web.Translators.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.NET.Web.Controllers
{
    /// <summary>
    /// Api controller for work with OpenAI.Api.
    /// </summary>
    public class ApiController : BaseController, IApiController
    {
        private readonly HttpClient _client;
        private readonly ITranslator _translator;

        /// <summary>
        /// A constructor that initializes all fields.
        /// </summary>
        public ApiController(IConfiguration configuration)
        {
            _client = InitHttpClient(configuration);

            _translator = new GoogleTranslator(
                configuration);
        }

        /// <summary>
        /// Initialization of HttpClient.
        /// </summary>
        /// <returns>Initiated HttpClient.</returns>
        private static HttpClient InitHttpClient(
            IConfiguration configuration)
        {
            string host = configuration["OpenAI:Uri"];
            HttpClient client = new()
            {
                BaseAddress =
                    new Uri(host.EndsWith('/') ? host : host + "/")
            };
            
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    configuration["OpenAI:SecretKey"]);
            client.DefaultRequestHeaders.Add(
                "OpenAI-Organization",
                $"{configuration["OpenAI:OrganizationKey"]}");
            client.DefaultRequestHeaders.Add(
                "User-Agent",
                "dotnet_openai_api");

            return client;
        }

        [HttpPost("/Api/Complete")] 
        [Authorize(Roles = Permission.CanCallApi)]
        public async Task<IActionResult> CompleteAsync(
            CompleteRequest request)
        {
            return await SendResponseAsync(
                request,
                InvokeCompleteAsync,
                "Exception in using OpenAI API");
        }

        /// <summary>
        /// Async method that executing in
        /// <see cref="BaseController.SendResponseAsync{T}(T, Action{T}, string)"/>.
        /// </summary>
        /// <returns>Json content with <see cref="CompleteResponse"/> body.</returns>
        [NonAction]
        private async Task<IActionResult> InvokeCompleteAsync(
            CompleteRequest request,
            Response response)
        {
            request.Prompt =
                await _translator.TranslateIntoEnglishAsync(
                    request.Prompt,
                    request.RequestLanguage);

            string completion =
                await _translator.TranslateFromEnglishAsync(
                    await TryExecuteCompletionsAsync(request),
                    request.ResponseLanguage);

            response.Body = new CompleteResponse()
            {
                Completion = completion.TrimStart('\n').TrimEnd('\n')
            };

            return Content(JsonConvert.SerializeObject(
                response,
                Formatting.Indented));
        }

        /// <summary>
        /// Async sending a request to OpenAI.Api.
        /// </summary>
        /// <returns>Completion.</returns>
        [NonAction]
        private async Task<string> TryExecuteCompletionsAsync(
            CompleteRequest request)
        {
            string content = JsonConvert
                .SerializeObject(request, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            HttpResponseMessage responseMessage =
                await _client.PostAsync(
                    $"{request.Engine}/completions",
                    new StringContent(
                        content,
                        Encoding.UTF8,
                        "application/json"));

            if (responseMessage.IsSuccessStatusCode)
            {
                byte[] bytes =
                    await responseMessage.Content.ReadAsByteArrayAsync();

                return JObject.Parse(Encoding.UTF8.GetString(bytes))
                     .SelectToken("choices[0].text")
                     .ToString();
            }
            else
            {
                throw new Exception(responseMessage.ReasonPhrase);
            }
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI.NET.Models;
using OpenAI.NET.Models.Api.Complete;
using OpenAI.NET.Web.EntityFrameworkCore.Models;
using OpenAI.NET.Web.Services;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.NET.Web.Controllers
{
    public class ApiController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly HttpClient _client;

        public ApiController(IConfiguration configuration)
        {
            _configuration = configuration;

            _client = new HttpClient();
            _client.DefaultRequestHeaders
                .Add("Authorization", $"Bearer {_configuration["OpenAI:SecretKey"]}");
            _client.DefaultRequestHeaders
                .Add("OpenAI-Organization", $"{_configuration["OpenAI:OrganizationKey"]}");
            _client.DefaultRequestHeaders
                .Add("User-Agent", "dotnet_openai_api");
        }

        [HttpPost, Route("/Api/Complete"), Authorize(Roles = Permission.CanCallApi)]
        public async Task<IActionResult> CompleteAsync(
            CompleteRequestParameters request)
        {
            return await BuildCompleteResponseAsync(request);
        }

        [NonAction]
        private async Task<IActionResult> BuildCompleteResponseAsync(
            CompleteRequestParameters request)
        {
            ResponseBuilder builder = new(typeof(CompleteRequestParameters), request);
            Response response = builder.Build();

            if (response.Exceptions is not null)
            {
                return BadRequest(JsonConvert.SerializeObject(
                    response,
                    Formatting.Indented));
            }

            string message = string.Empty;

            try
            {
                message = await TryExecuteCompletionsAsync(request);
            }
            catch (Exception ex)
            {
                builder.AddException(
                    "Exception in using OpenAI API",
                    ex.Message);
            }

            if (response.Exceptions is not null)
            {
                return BadRequest(JsonConvert.SerializeObject(
                    response,
                    Formatting.Indented));
            }

            response.Body = new CompleteResponseBody()
            {
                Completion = message
            };

            return Content(JsonConvert.SerializeObject(
                response,
                Formatting.Indented));
        }

        [NonAction]
        private async Task<string> TryExecuteCompletionsAsync(CompleteRequestParameters request)
        {
            string parameters = JsonConvert
                .SerializeObject(request, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            HttpResponseMessage responseMessage =
                await _client.PostAsync(
                    $"https://api.openai.com/v1/engines/{request.Engine}/completions",
                    new StringContent(parameters, Encoding.UTF8, "application/json"));

            if (responseMessage.IsSuccessStatusCode)
            {
                byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();

                return JObject.Parse(Encoding.UTF8.GetString(bytes))
                     .SelectToken("choices[0].text")
                     .ToString()
                     .TrimStart('\n')
                     .TrimEnd('\n');
            }
            else
            {
                throw new Exception(responseMessage.ReasonPhrase);
            }
        }
    }
}
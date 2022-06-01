using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI.NET.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.NET.Controllers
{
    public class ApiController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly HttpClient _client;

        public ApiController(IConfiguration configuration)
        {
            _configuration = configuration;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["SecretKey"]}");
            _client.DefaultRequestHeaders.Add("OpenAI-Organization", $"{_configuration["OrganizationKey"]}");
            _client.DefaultRequestHeaders.Add("User-Agent", "dotnet_openai_api");
        }

        [HttpGet]
        [Route("/Complete")]
        public async Task<IActionResult> Complete(
            Request request)
        {
            request.SetDefaultValues();

            return Content(JsonConvert.SerializeObject(
                await GetResponseBody(request),
                Formatting.Indented));
        }

        [NonAction]
        private async Task<Response> GetResponseBody(
            Request request)
        {
            Response response = new();

            foreach ((string, object) parameter in new List<(string, object)>()
            { 
                ("action", request.Action),
                ("engine", request.Engine),
                ("prompt", request.Prompt),
                ("temperature", request.Temperature),
                ("maxTokens", request.MaxTokens),
                ("topP", request.TopP),
                ("frequencyPenalty", request.FrequencyPenalty),
                ("presencePenalty", request.PresencePenalty),
                ("bestOf", request.BestOf)
            })
            {
                if (parameter.Item2 is not null)
                {
                    response.Parameters ??= new Dictionary<string, string>();

                    response.Parameters.Add(
                        string.Concat(
                            parameter.Item1[0].ToString().ToUpper(),
                            parameter.Item1.AsSpan(1)),
                        parameter.Item2.ToString());
                }
                else
                {
                    AddError(
                        ref response,
                        "One or more of the specified parameters was missing or invalid",
                        $"Parameter {parameter.Item1} can not be empty or null");
                }
            }

            if (response.Errors is not null)
            {
                return response;
            }

            string message = string.Empty;

            try
            {
                message = await SendRequest(request);
            }
            catch (Exception ex)
            {
                AddError(
                    ref response,
                    "Exception in OpenAI API using",
                    ex.Message);
            }

            if (response.Errors is not null)
            {
                return response;
            }

            response.Message = message;

            return response;
        }

        [NonAction]
        private async Task<string> SendRequest(Request request)
        {
            string parameters = JsonConvert.SerializeObject(request, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            HttpResponseMessage responseMessage =
                await _client.PostAsync(
                    $"https://api.openai.com/v1/engines/{request.Engine}/completions",
                    new StringContent(parameters, Encoding.UTF8, "application/json"));

            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();

           return JObject.Parse(Encoding.UTF8.GetString(bytes))
                .SelectToken("choices[0].text")
                .ToString()
                .TrimStart('\n')
                .TrimEnd('\n');
        }

        [NonAction]
        private static void AddError(ref Response response, string message, string errorMessage)
        {
            if (response.Errors is null)
            {
                response.Errors = new List<string>();
                response.Message = message;
            }

            response.Errors.Add(errorMessage);
        }
    }
}
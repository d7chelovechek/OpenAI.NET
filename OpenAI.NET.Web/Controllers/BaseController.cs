using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenAI.NET.Models.Response;
using OpenAI.NET.Web.Builders;
using System;
using System.Threading.Tasks;

namespace OpenAI.NET.Web.Controllers
{
    /// <summary>
    /// Base controller class that contains methods to help process requests.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Receive result of request.
        /// </summary>
        /// <returns>Json content</returns>
        internal delegate Task<IActionResult> Action<T>(
            T request,
            Response response);

        /// <summary>
        /// Async processing a request and sending a response.
        /// </summary>
        /// <returns>Json content.</returns>
        internal async Task<IActionResult> SendResponseAsync<T>(
            T request,
            Action<T> action,
            string exceptionTitle)
        {
            ResponseBuilder<T> builder = new(request);
            Response response = builder.Build();

            if (response.Exceptions is not null)
            {
                return BadRequest(
                    JsonConvert.SerializeObject(response,
                    Formatting.Indented));
            }

            try
            {
                return await action.Invoke(request, response);
            }
            catch (Exception ex)
            {
                builder.AddException(
                    exceptionTitle,
                    ex.Message);

                return BadRequest(JsonConvert.SerializeObject(
                    response,
                    Formatting.Indented));
            }
        }
    }
}
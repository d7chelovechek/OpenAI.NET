using Newtonsoft.Json;
using OpenAI.NET.Models.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.NET.Lib.Controllers
{
    /// <summary>
    /// Base controller class that contains methods to help process requests.
    /// </summary>
    public class BaseController
    {
        /// <summary>
        /// Creating a dictionary of parameters to send a request to OpenAI.NET.Web.
        /// </summary>
        /// <returns>Parameters Dictionary.</returns>
        internal static Dictionary<string, string> GetContent<T>(
            T request)
        {
            Dictionary<string, string> content = new();

            foreach (PropertyInfo property in
                request.GetType().GetProperties())
            {
                object value = property.GetValue(request);

                if (value is not null)
                {
                    content.Add(
                        property.Name,
                        value is double ?
                            value.ToString().Replace(',', '.') :
                            value.ToString());
                }
            }

            return content;
        }

        /// <summary>
        /// Throwing one exception based on all exceptions returned by OpenAI.NET.Web.
        /// </summary>
        /// <returns>An exception that stores all exceptions in message.</returns>
        internal static Exception GetException(
            string body,
            List<string> exceptions)
        {
            StringBuilder message = new(body);

            if (exceptions is not null)
            {
                message.Append(": ");
                foreach (string exception in exceptions)
                {
                    message.Append($"{exception}; ");
                }
            }

            return new Exception(message.ToString().TrimEnd(' ', ';'));
        }

        /// <summary>
        /// Async deserializing response from OpenAI.NET.Web.
        /// </summary>
        /// <returns>Returns response body from OpenAI.NET.Web.</returns>
        internal static async Task<T> DeserializeResponseAsync<T>(
            HttpResponseMessage responseMessage)
        {
            Response response =
                JsonConvert.DeserializeObject<Response>(
                    await responseMessage.Content.ReadAsStringAsync());

            if (responseMessage.IsSuccessStatusCode)
            {
                if (response.Exceptions is null)
                {
                    return JsonConvert.DeserializeObject<T>(
                        response.Body.ToString());
                }
                else
                {
                    throw GetException(
                        response.Body.ToString(),
                        response.Exceptions);
                }
            }
            else
            {
                throw GetException(
                    responseMessage.ReasonPhrase,
                    null);
            }
        }
    }
}
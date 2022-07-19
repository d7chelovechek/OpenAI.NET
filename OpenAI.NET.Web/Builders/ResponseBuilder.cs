using OpenAI.NET.Models.Response;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenAI.NET.Web.Builders
{
    /// <summary>
    /// Builder to build response.
    /// </summary>
    public class ResponseBuilder<T>
    {
        private readonly Response _response;

        private readonly T _request;

        /// <summary>
        /// A constructor that initializes all fields.
        /// </summary>
        public ResponseBuilder(T request)
        {
            _response = new Response();

            _request = request;
        }

        /// <summary>
        /// Building response.
        /// </summary>
        /// <returns>Builded response.</returns>
        public Response Build()
        {
            CheckParameters();

            return _response;
        }

        /// <summary>
        /// Checking all request parameters.
        /// </summary>
        private void CheckParameters()
        {
            foreach (PropertyInfo property in
                _request.GetType().GetProperties())
            {
                object value = property.GetValue(_request);

                if (value is Array array)
                {
                    foreach (object item in array)
                    {
                        CheckParameter(property, item);
                    }
                }
                else
                {
                    CheckParameter(property, value);
                }
            }
        }

        /// <summary>
        /// Checking request parameter.
        /// </summary>
        private void CheckParameter(
            PropertyInfo property,
            object value)
        {
            try
            {
                if (value is not null)
                {
                    _response.Parameters ??= new List<Parameter>();

                    _response.Parameters.Add(new Parameter()
                    {
                        Name = property.Name,
                        Value = value.ToString()
                    });
                }
                else
                {
                    throw new Exception(
                        $"Parameter {property.Name} " +
                        $"can not be empty or null");
                }
            }
            catch (Exception ex)
            {
                AddException(
                    "Exception in parameters checking:" +
                    " one or more of specified" +
                    " parameters was missing or invalid",
                    ex.Message);
            }
        }

        /// <summary>
        /// Adding an exception to response.
        /// </summary>
        public void AddException(
            string exceptionBody,
            string exceptionMessage)
        {
            if (_response.Body is null)
            {
                _response.Body = exceptionBody;
            }

            if (_response.Exceptions is null)
            {
                _response.Exceptions = new List<string>();
            }

            _response.Exceptions.Add(exceptionMessage);
        }
    }
}
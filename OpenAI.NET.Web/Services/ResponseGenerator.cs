using OpenAI.NET.Web.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenAI.NET.Web.Services
{
    public class ResponseBuilder
    {
        private readonly Response _response;

        private readonly Type _type;
        private readonly object _instance;

        public ResponseBuilder(Type type, object instance)
        {
            _response = new Response();

            _type = type;
            _instance = instance;
        }

        public Response Build()
        {
            CheckParameters();

            return _response;
        }

        private void CheckParameters()
        {
            foreach (PropertyInfo property in _type.GetProperties())
            {
                object value = property.GetValue(_instance);

                if (value is not null)
                {
                    _response.Parameters ??= new Dictionary<string, string>();

                    _response.Parameters.Add(
                        property.Name,
                        value.ToString());
                }
                else
                {
                    AddError(
                        "One or more of the specified parameters was missing or invalid",
                        $"Parameter {property.Name} can not be empty or null");
                }
            }
        }

        public void AddError(string message, string errorMessage)
        {
            if (_response.Body is null)
            {
                _response.Body = message;
            }

            if (_response.Errors is null)
            {
                _response.Errors = new List<string>();
            }

            _response.Errors.Add(errorMessage);
        }
    }
}
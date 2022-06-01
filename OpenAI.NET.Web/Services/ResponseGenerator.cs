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

        private void CheckParameter(PropertyInfo property, object value)
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
                    throw new Exception($"Parameter {property.Name} can not be empty or null");
                }
            }
            catch (Exception ex)
            {
                AddException(
                    "Exception in parameters checking:" +
                    " one or more of the specified parameters was missing or invalid",
                    ex.Message);
            }
        }

        public void AddException(string exceptionBody, string exceptionMessage)
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
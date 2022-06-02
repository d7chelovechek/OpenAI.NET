using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenAI.NET.Lib.Services
{
    public class LibHelper
    {
        public static Dictionary<string, string> GetContent(Type type, object instance)
        {
            Dictionary<string, string> content = new Dictionary<string, string>();

            foreach (PropertyInfo property in type.GetProperties())
            {
                object value = property.GetValue(instance);

                if (!(value is null))
                {
                    content.Add(
                        property.Name,
                        (value is double) ?
                            value.ToString().Replace(',', '.') : value.ToString());
                }
            }

            return content;
        }

        public static Exception GetException(string body, List<string> exceptions)
        {
            string message = body + ": ";
            foreach (string exception in exceptions)
            {
                message += exception + "; ";
            }
            message = message.TrimEnd(' ', ';');

            return new Exception(message);
        }
    }
}
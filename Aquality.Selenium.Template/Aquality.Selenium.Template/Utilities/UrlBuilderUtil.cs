using Newtonsoft.Json;
using Se.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aquality.Selenium.Template.Utilities
{
    class UrlBuilderUtil
    {
        private static string urlAPI;

        static UrlBuilderUtil()
        {
            urlAPI = ReadConfig.GetParam("configApi", "baseUrl");
        }

        public static Uri UriBuilder<T>(string method, T body, params string[] names) where T : class
        {
            string url = urlAPI + $"{method}";

            UrlBuilder urlBuilder = new UrlBuilder(url);

            if (names == null)
            {
                urlBuilder.SetQueryParams(body);
            }
            else
            {
                foreach (KeyValuePair<string, string> pair in GetParams(body, names))
                {
                    urlBuilder.SetQueryParam(pair.Key, pair.Value);
                }
            }

            return urlBuilder;
        }

        public static Dictionary<string, string> GetParams<T>(T obj, string[] names) where T : class
        {
            Type type = obj.GetType();
            Dictionary<string, string> dict = new Dictionary<string, string>();

            var result = string.Empty;

            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props.Where(p => names.Contains(p.Name)))
            {
                foreach (object attr in prop.GetCustomAttributes(true))
                {
                    var value = prop.GetValue(obj).ToString();
                    dict.Add((attr as JsonPropertyAttribute).PropertyName, value);
                }
            }
            return dict;
        }
    }
}

using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace Aquality.Selenium.Template.Utilities
{
    class APIUtils
    {
        private static string urlAPI;

        static APIUtils()
        {
            urlAPI = ReadConfig.GetParam("configApi", "baseUrl");
        }

        private static T GetData<T>(WebResponse response) where T : class
        {
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);

            var result = sr.ReadToEnd();
            try
            {
                return string.IsNullOrEmpty(result) || result == "{}" ? null : JsonConvert.DeserializeObject<T>(result);
            }
            catch
            {
                return null;
            }
        }

        private static string GetToken(WebResponse response)        {
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);

            return sr.ReadToEnd();
        }

        public static int GetStatusCode(WebResponse response)
        {
            return (int)(response as HttpWebResponse).StatusCode;
        }

        public static int GetToken(string url, out string result)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = HttpMethod.POST.ToString();

                WebResponse response = request.GetResponse();
                result = GetToken(response);

                return GetStatusCode(response);
            }
            catch (WebException e)
            {
                result = null;
                return default;
            }
        }

        public static int Post<T>(string url, out T result) where T : class
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = HttpMethod.POST.ToString();

                WebResponse response = request.GetResponse();
                result = GetData<T>(response);

                return GetStatusCode(response);
            }
            catch (WebException e)
            {
                var statusCode = GetStatusCode(e.Response);

                if (statusCode == (int)HttpStatusCode.NotFound)
                {
                    result = GetData<T>(e.Response);
                    return statusCode;
                }
                result = null;
                return default;
            }
        }
    }

    public enum HttpMethod
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
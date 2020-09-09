using System;

namespace Aquality.Selenium.Template.Utilities
{
    public class BaseAuthUtil
    {
        public static Uri UriBuilder(string url, string username, string password)
        {
            UriBuilder uriBuilder = new UriBuilder(url);
            uriBuilder.UserName = username;
            uriBuilder.Password = password;
            Uri uri = uriBuilder.Uri;
            return uri;
        }
    }
}

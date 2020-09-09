using Aquality.Selenium.Browsers;
using OpenQA.Selenium;

namespace Aquality.Selenium.Template.Utilities
{
    public class CookieUtil
    {
        public static void AddCookie(CookieAction cookieAction, string keyCookie, string valueCookie)
        {
            var cookie = AqualityServices.Browser.Driver.Manage().Cookies;

            if (cookieAction.Equals(CookieAction.Add))
            {
                cookie.AddCookie(new Cookie(keyCookie, valueCookie));
            }
        }
    }

    public enum CookieAction
    {
        Add
    }
}
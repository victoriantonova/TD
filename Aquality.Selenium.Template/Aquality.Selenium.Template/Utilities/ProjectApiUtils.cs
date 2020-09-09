using Aquality.Selenium.Template.Models;
using System.Collections.Generic;

namespace Aquality.Selenium.Template.Utilities
{
    public class ProjectApiUtils
    {
        public static string GenerateToken(Token variant)
        {
            APIUtils.GetToken(UrlBuilderUtil.UriBuilder("token/get", variant, nameof(Token.Variant)).ToString(), out string token);
            return token;
        }

        public static List<Test> GetTestToJSON(Project project)
        {
            APIUtils.Post(UrlBuilderUtil.UriBuilder("test/get/json", project, nameof(Project.Id)).ToString(), out List<Test> tests);
            return tests;
        }
    }
}
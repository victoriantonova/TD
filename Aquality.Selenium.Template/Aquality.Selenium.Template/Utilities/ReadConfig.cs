using System;
using System.IO;
using System.Xml;

namespace Aquality.Selenium.Template.Utilities
{
    public static class ReadConfig
    {
        public static string GetParam(string filename, string paramName)
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(exePath, $"{filename}.xml");
            XmlDocument document = new XmlDocument();
            document.Load(path);

            XmlElement xRoot = document.DocumentElement;
            string valueParam = "";
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Name == paramName)
                {
                    valueParam = xnode.InnerText;
                }
            }
            return valueParam;
        }
    }
}

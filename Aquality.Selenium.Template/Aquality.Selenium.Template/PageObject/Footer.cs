using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace Aquality.Selenium.Template.PageObject
{
    public class Footer : Form
    {
        public Footer() : base(By.XPath("//footer"), "Footer")
        {
        }

        ILabel FooterLabel => ElementFactory.GetLabel(By.XPath("//footer//*[contains(text(), 'Version:')]"), "Footer label");
        
        public string GetVariant()
        {
            var footerText = FooterLabel.Text;
            
            string pattern = @"\d{1,}";
            Regex rgx = new Regex(pattern);

            string variant = string.Empty;

            foreach (Match match in rgx.Matches(footerText))
                variant = match.Value;

            return variant;
        }
    }
}

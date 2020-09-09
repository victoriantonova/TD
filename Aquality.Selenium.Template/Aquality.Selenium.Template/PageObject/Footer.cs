using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Aquality.Selenium.Template.PageObject
{
    public class Footer : Form
    {
        public Footer() : base(By.XPath("//footer"), "Footer")
        {
        }

        ILabel FooterLabel => ElementFactory.GetLabel(By.XPath("//footer//span"), "Footer label");
        
        public string GetVariant()
        {
            var footerText = FooterLabel.Text;
            return footerText.Substring(footerText.IndexOf(" ") + 1);
        }
    }
}

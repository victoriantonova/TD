using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Aquality.Selenium.Template.PageObject
{
    public class Header : Form
    {
        public Header() : base(By.XPath("//*[@class='breadcrumb']"), "Header")
        {
        }

        public void ReturnToHomePage()
        {
            ILink homeLink = ElementFactory.GetLink(By.XPath("//*[@class='breadcrumb']//a[contains(text(), 'Home')]"), "Home Link");
            homeLink.ClickAndWait();
        }
    }
}

using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Aquality.Selenium.Template.PageObject
{
    public class HomePage : Form
    {
        public HomePage() : base(By.XPath("//div[contains(text(), 'Available projects')]"), "All project page")
        {
        }

        private const string project = "//div[@class='list-group']/a[contains(text(), '{0}')]";

        public Footer Footer
        {
            get
            {
                return new Footer();
            }
        }

        public Header Header
        {
            get
            {
                return new Header();
            }
        }

        public AddProjectWindow AddProjectWindow
        {
            get
            {
                return new AddProjectWindow();
            }
        }

        IButton AddProjectButton => ElementFactory.GetButton(By.XPath("//button[@data-target='#addProject']"), "Add Project Button");

        public void OpenProject(string projectName)
        {
            IButton projectItemButton = ElementFactory.GetButton(By.XPath(string.Format(project, projectName)), $"{projectName} projectName Button");
            projectItemButton.ClickAndWait();
        }

        public bool IsProjectDisplayed(string projectName)
        {
            IButton projectItemButton = ElementFactory.GetButton(By.XPath(string.Format(project, projectName)), $"{projectName} projectName Button");
            return projectItemButton.State.IsDisplayed;
        }

        public void OpenAddProjectWindow()
        {
            AddProjectButton.ClickAndWait();
        }
    }
}
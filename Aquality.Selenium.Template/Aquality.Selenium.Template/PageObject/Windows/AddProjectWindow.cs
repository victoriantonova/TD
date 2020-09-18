using Aquality.Selenium.Browsers;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Aquality.Selenium.Template.PageObject
{
    public class AddProjectWindow : Form
    {
        public AddProjectWindow() : base(By.XPath("//div[@class='modal-content']"), "Add project modal-window")
        {
        }

        private readonly string addProjectFrameName = "addProjectFrame";
        private readonly string closeMethodName = "closePopUp()";
        ITextBox ProjectNameTextBox => ElementFactory.GetTextBox(By.XPath("//input[@name='projectName']"), "Project Name TextBox");
        IButton SaveProjectButton => ElementFactory.GetButton(By.XPath("//button[@type='submit']"), "Save Project Button");
        ILabel SuccessLabel => ElementFactory.GetLabel(By.XPath("//div[contains(@class, 'alert-success')]"), "Success Label");

        public void AddProject(string projectName)
        {
            AqualityServices.Browser.Driver.SwitchTo().Frame(addProjectFrameName);
            ProjectNameTextBox.ClearAndType(projectName);
            SaveProjectButton.ClickAndWait();
            AqualityServices.Browser.Driver.SwitchTo().DefaultContent();
        }

        public bool SuccessIsDisplayed()
        {
            AqualityServices.Browser.Driver.SwitchTo().Frame(addProjectFrameName);
            bool result = SuccessLabel.State.IsDisplayed;
            AqualityServices.Browser.Driver.SwitchTo().DefaultContent();
            return result;
        }

        public void ClosePopUp()
        {
            AqualityServices.Browser.ExecuteScript(closeMethodName);
        }
    }
}

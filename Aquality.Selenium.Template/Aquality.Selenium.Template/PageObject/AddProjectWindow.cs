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

        ITextBox ProjectNameTextBox => ElementFactory.GetTextBox(By.XPath("//input[@name='projectName']"), "Project Name TextBox");
        IButton SaveProjectButton => ElementFactory.GetButton(By.XPath("//button[@type='submit']"), "Save Project Button");
        ILabel SuccessLabel => ElementFactory.GetLabel(By.XPath("//div[@class='alert alert-success']"), "Success Label");

        public void AddProject(string projectName)
        {
            AqualityServices.Browser.Driver.SwitchTo().Frame("addProjectFrame");
            ProjectNameTextBox.ClearAndType(projectName);
            SaveProjectButton.ClickAndWait();
        }

        public bool SuccessIsDisplayed()
        {
            return SuccessLabel.State.IsDisplayed;
        }

        public void ClosePopUp()
        {
            AqualityServices.Browser.Refresh();
            AqualityServices.Browser.ExecuteScript("closePopUp()");
        }
    }
}

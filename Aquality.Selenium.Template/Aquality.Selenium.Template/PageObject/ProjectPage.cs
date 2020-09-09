using Aquality.Selenium.Browsers;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using Aquality.Selenium.Template.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Aquality.Selenium.Template.PageObject
{
    public class ProjectPage : Form
    {
        public ProjectPage() : base(By.XPath("//div[@class='panel panel-default']//*[contains(text(), 'Total tests progress')]"), "Project page")
        {
        }

        public Header Header
        {
            get
            {
                return new Header();
            }
        }

        private readonly string tableLocator = "//table[@class='table']//tr";

        private int CountRow()
        {
            var elementFactory = AqualityServices.Get<IElementFactory>();
            var rowsLabels = elementFactory.FindElements<ILabel>(By.XPath(tableLocator));
            return rowsLabels.Count;
        }

        public List<Test> GetTests()
        {
            List<Test> tests = new List<Test>();

            for(int i = 2; i <= CountRow(); i++)
            {
                ILabel TestNameLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{1}]"), "Test Name Label");
                ILabel MethodLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{2}]"), "Method Name Label");
                ILabel StatusLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{3}]"), "Status Label");
                ILabel StartTimeLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{4}]"), "Start Time Label");
                ILabel EndTimeLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{5}]"), "End Time Label");
                ILabel DurationLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{6}]"), "Duration Label");

                Test test = new Test
                {
                    Name = TestNameLabel.Text,
                    Method = MethodLabel.Text,
                    Status = StatusLabel.Text,
                    StartTime = string.IsNullOrEmpty(StartTimeLabel.Text) ? (DateTime?)null : Convert.ToDateTime(StartTimeLabel.Text),
                    EndTime = string.IsNullOrEmpty(EndTimeLabel.Text) ? (DateTime?)null : Convert.ToDateTime(EndTimeLabel.Text),
                    Duration = DurationLabel.Text
                };

                tests.Add(test);
            }

            return tests;
        }

        public bool FindTestByName(string testName)
        {
            ILabel testNameLabel = ElementFactory.GetLabel(By.XPath($"//table[@id='allTests']//a[contains(text(), '{testName}')]"), "Test Name Label");
            return testNameLabel.State.IsDisplayed;          
        }

        public void OpenTest(string testName)
        {
            ILabel testNameLabel = ElementFactory.GetLabel(By.XPath($"//table[@id='allTests']//a[contains(text(), '{testName}')]"), "Test Name Label");
            testNameLabel.ClickAndWait();
        }
    }
}

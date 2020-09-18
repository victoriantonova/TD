using Aquality.Selenium.Browsers;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using Aquality.Selenium.Template.Models;
using Aquality.Selenium.Template.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Aquality.Selenium.Template.PageObject
{
    public class ProjectPage : Form
    {
        public ProjectPage() : base(By.XPath("//div[contains(text(), 'Total tests progress')]"), "Project page")
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

        private int GetIndexCell(CellName cellName)
        {
            var elementFactory = AqualityServices.Get<IElementFactory>();
            var rowsLabels = elementFactory.FindElements<ILabel>(By.XPath("//table[@class='table']//preceding-sibling::th"));

            foreach(ILabel element in rowsLabels)
            {
                if(element.Text.Contains(EnumHelper.StringValueOf(cellName)))
                {
                    return rowsLabels.IndexOf(element) + 1;
                }
            }
            return default;
        }

        public List<Test> GetTests()
        {
            List<Test> tests = new List<Test>();

            int indexTestName = GetIndexCell(CellName.TestName);
            int indexMethod = GetIndexCell(CellName.Method);
            int indexStatus = GetIndexCell(CellName.Status);
            int indexStartTime = GetIndexCell(CellName.StartTime);
            int indexEndTime = GetIndexCell(CellName.EndTime);
            int indexDuration = GetIndexCell(CellName.Duration);

            for (int i = 2; i <= CountRow(); i++)
            {
                ILabel TestNameLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{indexTestName}]"), "Test Name Label");
                ILabel MethodLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{indexMethod}]"), "Method Name Label");
                ILabel StatusLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{indexStatus}]"), "Status Label");
                ILabel StartTimeLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{indexStartTime}]"), "Start Time Label");
                ILabel EndTimeLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{indexEndTime}]"), "End Time Label");
                ILabel DurationLabel = ElementFactory.GetLabel(By.XPath($"{tableLocator}[{i}]/td[{indexDuration}]"), "Duration Label");

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

    public enum CellName
    {
        [Description("Test name")]
        TestName,
        [Description("Test method")]
        Method,
        [Description("Latest test result")]
        Status,
        [Description("Latest test start time")]
        StartTime,
        [Description("Latest test end time")]
        EndTime,
        [Description("Latest test duration")]
        Duration
    }
}

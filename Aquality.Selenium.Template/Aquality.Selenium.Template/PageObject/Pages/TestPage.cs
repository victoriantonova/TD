using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using Aquality.Selenium.Template.Models;
using OpenQA.Selenium;
using System;

namespace Aquality.Selenium.Template.PageObject
{
    public class TestPage : Form
    {
        public TestPage(string testName) : base(By.XPath($"//li[contains(text(), '{testName}')]"), $"Test {testName} page")
        {
        }

        ILabel TestNameLabel => ElementFactory.GetLabel(By.XPath("//*[contains(text(), 'Test name')]/following::p"), "Test name label");
        ILabel TestMethodNameLabel => ElementFactory.GetLabel(By.XPath("//*[contains(text(), 'Test method name')]/following::p"), "Test method name label");
        ILabel StatusLabel => ElementFactory.GetLabel(By.XPath("//*[contains(text(), 'Status')]/following::span"), "Status label");
        ILabel StartTimeLabel => ElementFactory.GetLabel(By.XPath("//*[contains(text(), 'Start time')]"), "Start time label");
        ILabel EndTimeLabel => ElementFactory.GetLabel(By.XPath("//*[contains(text(), 'End time')]"), "End time label");
        ILabel EnvironmentLabel => ElementFactory.GetLabel(By.XPath("//*[contains(text(), 'Environment')]/following::p"), "Environment label");
        ILabel BrowserLabel => ElementFactory.GetLabel(By.XPath("//*[contains(text(), 'Browser')]/following::p"), "Browser label");
        ILabel LogLabel => ElementFactory.GetLabel(By.XPath("//div[contains(text(), 'Logs')]/following::td"), "Log label");
        ILabel AttachmentLabel => ElementFactory.GetLabel(By.XPath("//div[contains(text(), 'Attachments')]/following::table//a"), "Attachment");
        
        private DateTime GetDate(string date)
        {
            return Convert.ToDateTime(date.Substring(date.IndexOf(":") + 2));
        }

        public Test GetTest()
        {
            return new Test
            {
                Name = TestNameLabel.Text,
                Method = TestMethodNameLabel.Text,
                Status = StatusLabel.Text,
                StartTime = GetDate(StartTimeLabel.Text),
                EndTime = GetDate(EndTimeLabel.Text),
                Environment = EnvironmentLabel.Text,
                Browser = BrowserLabel.Text
            };
        }

        public string GetAttachment()
        {
            var attachment = AttachmentLabel.GetAttribute("href");
            var index = attachment.IndexOf(",") + 1;
            return attachment.Substring(index);
        }

        public string GetLog()
        {
            return LogLabel.Text;
        }
    }
}
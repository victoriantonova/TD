using Aquality.Selenium.Browsers;
using Aquality.Selenium.Core.Logging;
using Aquality.Selenium.Template.Models;
using Aquality.Selenium.Template.PageObject;
using Aquality.Selenium.Template.Tests.Enum;
using Aquality.Selenium.Template.Utilities;
using DAL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestRail.Enums;
using TestRail.Models;
using TestRail.Util;

namespace Aquality.Selenium.Template.Tests.Tests
{
    public class Autotest
    {
        private const string cookieName = "token";
        private const int statusId = 3;
        private const int sessionId = 17;
        private const int caseId = 11345480;
        private const int testRunId = 45755;

        protected static Logger Logger => AqualityServices.Logger;

        [TestCase("Nexage")]
        public void FirstVariant(string projectName)
        {
            Logger.Info("step 1");
            Token token = new Token
            {
                Variant = ReadConfig.GetParam("configApp", "variant")
            };
            token.Value = ProjectApiUtils.GenerateToken(token);
            Assert.IsNotNull(token.Value, "Token not generated");

            Logger.Info("step 2");
            AqualityServices.Browser.GoTo(BaseAuthUtil.UriBuilder(ReadConfig.GetParam("configApp", "baseUrl"), ReadConfig.GetParam("configApp", "username"), ReadConfig.GetParam("configApp", "password")).ToString());
            AqualityServices.Browser.Maximize();
            CookieUtil.AddCookie(CookieAction.Add, cookieName, token.Value);
            AqualityServices.Browser.Refresh();
            HomePage homePage = new HomePage();
            Assert.IsTrue(homePage.IsDisplayed, "HomePage is not open");
            Assert.AreEqual(homePage.Footer.GetVariant(), token.Variant, "Variant does not match");

            Logger.Info("step 3");
            Project project = new Project
            {
                Name = projectName
            };
            homePage.OpenProject(project.Name);
            Operations operation = new Operations();
            project.Id = operation.GetProjectByName(project.Name).Id;
            List<Test> tests = ProjectApiUtils.GetTestToJSON(project);
            ProjectPage projectPage = new ProjectPage();
            List<Test> testsFromPage = projectPage.GetTests();
            var afterSortedTestsByDate = testsFromPage.OrderByDescending(p => p.StartTime);
            Assert.AreEqual(testsFromPage.Select(x => x.StartTime), afterSortedTestsByDate.Select(x => x.StartTime), "Tests not sorted by date");
            Assert.IsTrue(testsFromPage.All(x => tests.Select(y => y.Name).Contains(x.Name)), "Tests don't match");

            Logger.Info("step 4");
            projectPage.Header.ReturnToHomePage();
            homePage.OpenAddProjectWindow();
            Project createdProject = new Project
            {
                Name = DataUtil.RandomString(DateTime.Now.Second)
            };
            homePage.AddProjectWindow.AddProject(createdProject.Name);
            Assert.IsTrue(homePage.AddProjectWindow.SuccessIsDisplayed(), "Success label is not display");
            homePage.AddProjectWindow.ClosePopUp();
            Assert.IsFalse(homePage.AddProjectWindow.IsDisplayed, "Window is not closed");
            AqualityServices.Browser.Refresh();
            Assert.IsTrue(homePage.IsProjectDisplayed(createdProject.Name), "Project is not display");

            Logger.Info("step 5");
            homePage.OpenProject(createdProject.Name);
            DAL.Models.Test createdTest = new DAL.Models.Test
            {
                Name = DataUtil.RandomString(DateTime.Now.Second),
                StatusId = statusId,
                MethodName = DataUtil.RandomString(DateTime.Now.Second),
                SessionId = sessionId,
                StartTime = new DateTime(2020, 9, 9, 18, 30, 25),
                EndTime = new DateTime(2020, 9, 9, 18, 40, 25),
                Env = DataUtil.RandomString(DateTime.Now.Second),
                Browser = "chrome"
            };
            createdTest.ProjectId = operation.GetProjectByName(createdProject.Name).Id;
            operation.AddTest(createdTest);
            createdTest.Id = operation.GetTestByName(createdTest.ProjectId, createdTest.Name).Id;
            DAL.Models.Attachment attachment = new DAL.Models.Attachment
            {
                Content = AqualityServices.Browser.GetScreenshot(),
                ContentType = EnumHelper.StringValueOf(ImageContentType.Png),
                TestId = createdTest.Id
            };
            operation.AddAttachment(attachment);
            DAL.Models.Log log = new DAL.Models.Log
            {
                Content = DataUtil.RandomString(DateTime.Now.Second),
                IsExceprion = false,
                TestId = createdTest.Id
            };
            operation.AddLog(log);
            Assert.IsTrue(AqualityServices.ConditionalWait.WaitFor(() => projectPage.FindTestByName(createdTest.Name)));

            Logger.Info("step 6");
            projectPage.OpenTest(createdTest.Name);
            TestPage testPage = new TestPage(createdTest.Name);
            var selectedTest = testPage.GetTest();

            Assert.AreEqual(createdTest.Name, selectedTest.Name, "Name does not match");
            Assert.AreEqual(createdTest.MethodName, selectedTest.Method, "Method does not match");
            Assert.AreEqual(createdTest.StartTime.ToString(), selectedTest.StartTime.ToString(), "StartTime does not match");
            Assert.AreEqual(createdTest.EndTime, selectedTest.EndTime, "EndTime does not match");
            Assert.AreEqual(createdTest.Env, selectedTest.Environment, "Environment does not match");
            Assert.AreEqual(createdTest.Browser, selectedTest.Browser, "Browser does not match");
            Assert.AreEqual(Convert.ToBase64String(attachment.Content), testPage.GetAttachment(), "Attachment does not match");
            Assert.AreEqual(log.Content, testPage.GetLog(), "Log does not match");
        }

        [TearDown]
        public void TearDown()
        {
            string urlForAddAttach = $"{ReadConfig.GetParam("configTestRail", "baseUrl")}/index.php?/api/v2/add_attachment_to_result";
            TestRailUtil testRailUtil = new TestRailUtil(ReadConfig.GetParam("configTestRail", "baseUrl"), ReadConfig.GetParam("configTestRail", "username"), ReadConfig.GetParam("configTestRail", "password"));

            TestResult testResult = new TestResult();

            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                testResult.StatusId = (int)Status.Passed;
                testResult.Comment = "Passed";
            }
            else if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                testResult.StatusId = (int)Status.Failed;
                testResult.Comment = "Failed";
            }

            testResult = testRailUtil.AddResultForCase(testRunId, caseId, testResult);
            testRailUtil.AddImage($"{urlForAddAttach}/{testResult.Id}", AqualityServices.Browser.GetScreenshot());
        }
    }
}
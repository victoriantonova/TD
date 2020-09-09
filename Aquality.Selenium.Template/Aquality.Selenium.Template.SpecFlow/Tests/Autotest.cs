using Aquality.Selenium.Browsers;
using Aquality.Selenium.Template.Models;
using Aquality.Selenium.Template.PageObject;
using Aquality.Selenium.Template.Utilities;
using DAL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aquality.Selenium.Template.SpecFlow.Tests
{
    public class Autotest
    {
        [Test]
        public void Test()
        {
            //1
            Token token = new Token
            {
                Variant = ReadConfig.GetParam("configApp", "variant")
            };

            token.Value = ProjectApiUtils.GenerateToken(token);

            Assert.IsNotNull(token.Value, "Token not generated");

            //2
            AqualityServices.Browser.GoTo(BaseAuthUtil.UriBuilder(ReadConfig.GetParam("configApp", "baseUrl"), ReadConfig.GetParam("configApp", "username"), ReadConfig.GetParam("configApp", "password")).ToString());
            AqualityServices.Browser.Maximize();

            CookieUtil.AddCookie(CookieAction.Add, "token", token.Value);

            AqualityServices.Browser.Refresh();

            HomePage homePage = new HomePage();
            Assert.IsTrue(homePage.IsDisplayed, "HomePage is not open");

            Assert.AreEqual(homePage.Footer.GetVariant(), token.Variant, "Variant does not match");

            //3
            Project project = new Project
            {
                Name = "Nexage"
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

            //4
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
            Assert.IsTrue(homePage.ProjectIsDisplayed(createdProject.Name), "Project is not display");

            //5
            homePage.OpenProject(createdProject.Name);

            DAL.Models.Test createdTest = new DAL.Models.Test
            {
                Name = DataUtil.RandomString(DateTime.Now.Second),
                StatusId = 3,
                MethodName = DataUtil.RandomString(DateTime.Now.Second),
                SessionId = 17,
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
                ContentType = "image/png",
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

            //6
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
    }
}

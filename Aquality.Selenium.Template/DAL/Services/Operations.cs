using DAL.Context;
using DAL.Models;
using System.Linq;

namespace DAL
{
    public class Operations
    {
        ApplicationContext db = new ApplicationContext();

        public Project GetProjectByName(string projectName)
        {
            return db.Projects.FirstOrDefault(x => x.Name == projectName);
        }

        public Test GetTestByName(int projectId, string testName)
        {
            return db.Tests.Where(x => x.ProjectId == projectId).FirstOrDefault(x => x.Name == testName);
        }

        public void AddAttachment(Attachment attachment)
        {
            db.Attachments.Add(
                new Attachment
                {
                    Content = attachment.Content,
                    ContentType = attachment.ContentType,
                    TestId = attachment.TestId
                });
            db.SaveChanges();
        }

        public void AddLog(Log log)
        {
            db.Logs.Add(
                new Log
                {
                    Content = log.Content,
                    IsExceprion = log.IsExceprion,
                    TestId = log.TestId
                });

            db.SaveChanges();
        }

        public void AddTest(Test test)
        {
            db.Tests.Add(
                new Test
                {
                    Name = test.Name,
                    StatusId = test.StatusId,
                    MethodName = test.MethodName,
                    ProjectId = test.ProjectId,
                    SessionId = test.SessionId,
                    StartTime = test.StartTime,
                    EndTime = test.EndTime,
                    Env = test.Env,
                    Browser = test.Browser
                });

            db.SaveChanges();
        }
    }
}

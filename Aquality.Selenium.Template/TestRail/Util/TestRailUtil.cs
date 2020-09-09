using Gurock.TestRail;
using Newtonsoft.Json;
using System;
using System.IO;
using TestRail.Models;

namespace TestRail.Util
{
    public class TestRailUtil
    {
        APIClient client;
        public TestRailUtil(string url, string username, string password)
        {
            client = new APIClient(url);
            client.User = username;
            client.Password = password;
        }

        public TestRun AddRun(int prohectId, TestRun data)
        {
            var inputData = JsonConvert.SerializeObject(data);
            var result = client.SendPost($"add_run/{prohectId}", JsonConvert.DeserializeObject<TestRun>(inputData));

            var outputData = JsonConvert.SerializeObject(result);
            return JsonConvert.DeserializeObject<TestRun>(outputData);
        }

        public TestResult AddResultForCase(int runId, int caseId, TestResult data)
        {
            var inputData = JsonConvert.SerializeObject(data);
            var result = client.SendPost($"add_result_for_case/{runId}/{caseId}", JsonConvert.DeserializeObject<TestResult>(inputData));

            var outputData = JsonConvert.SerializeObject(result);
            return JsonConvert.DeserializeObject<TestResult>(outputData);
        }

        public TestResult AddAttachmentToResult(int caseId, byte[] image)
        {
            File.WriteAllBytes($@"{Environment.CurrentDirectory}\result.png", image);

            var result = client.SendPost($"add_attachment_to_result/{caseId}", $"{Environment.CurrentDirectory}\\result.png");

            var outputData = JsonConvert.SerializeObject(result);
            return JsonConvert.DeserializeObject<TestResult>(outputData);
        }
    }
}
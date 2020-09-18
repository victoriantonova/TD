using Gurock.TestRail;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
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

        public void AddImage(string url, byte[] image)
        {
            string screenshotName = "result.png";
            File.WriteAllBytes($@"{Environment.CurrentDirectory}\{screenshotName}", image);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";

            string auth = Convert.ToBase64String(
                Encoding.ASCII.GetBytes(
                    string.Format(
                        "{0}:{1}",
                        client.User,
                        client.Password
                    )
                )
            );

            request.Headers.Add("Authorization", "Basic " + auth);

            string boundary = String.Format("{0:N}", Guid.NewGuid());
            string filePath = $"{Environment.CurrentDirectory}\\{screenshotName}";

            request.ContentType = "multipart/form-data; boundary=" + boundary;

            using (MemoryStream postDataStream = new MemoryStream())
            using (StreamWriter postDataWriter = new StreamWriter(postDataStream))
            {
                postDataWriter.Write("\r\n--" + boundary + "\r\n");
                postDataWriter.Write("Content-Disposition: form-data; name=\"attachment\";"
                                + "filename=\"{0}\""
                                + "\r\nContent-Type: {1}\r\n\r\n",
                                Path.GetFileName(filePath),
                                Path.GetExtension(filePath));
                postDataWriter.Flush();

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        postDataStream.Write(buffer, 0, bytesRead);
                    }

                    postDataWriter.Write("\r\n--" + boundary + "--\r\n");
                    postDataWriter.Flush();


                    request.ContentLength = postDataStream.Length;

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        postDataStream.WriteTo(requestStream);
                    }
                }

                WebResponse response = request.GetResponse();
            }
        }
    }
}
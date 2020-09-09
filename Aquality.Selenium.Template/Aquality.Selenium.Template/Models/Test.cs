using Newtonsoft.Json;
using System;

namespace Aquality.Selenium.Template.Models
{
    public class Test
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "duration")]
        public string Duration { get; set; }
        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "startTime")]
        public DateTime? StartTime { get; set; }
        [JsonProperty(PropertyName = "endTime")]
        public DateTime? EndTime { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        public string Environment { get; set; }
        public string Browser { get; set; }

        public Project Project { get; set; }

        public override bool Equals(object obj)
        {
            return ((Test)obj).Duration == Duration && ((Test)obj).Method == Method
                && ((Test)obj).Name == Name && ((Test)obj).StartTime == StartTime
                && ((Test)obj).EndTime == EndTime && ((Test)obj).Status == Status;
        }
    }
}

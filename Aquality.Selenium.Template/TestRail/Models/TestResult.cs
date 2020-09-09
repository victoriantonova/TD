using Newtonsoft.Json;

namespace TestRail.Models
{
    public class TestResult
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "status_id")]
        public int StatusId { get; set; }
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }
        [JsonProperty(PropertyName = "attachment_id")]
        public int AttachmentId { get; set; }
    }
}

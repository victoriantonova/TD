using Newtonsoft.Json;

namespace TestRail.Models
{
    public class TestRun
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "suite_id")]
        public int SuiteId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}

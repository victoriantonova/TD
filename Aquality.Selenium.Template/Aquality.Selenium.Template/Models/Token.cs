using Newtonsoft.Json;

namespace Aquality.Selenium.Template.Models
{
    public class Token
    {
        [JsonProperty(PropertyName = "variant")]
        public string Variant { get; set; }
        [JsonProperty(PropertyName = "token")]
        public string Value { get; set; }
    }
}
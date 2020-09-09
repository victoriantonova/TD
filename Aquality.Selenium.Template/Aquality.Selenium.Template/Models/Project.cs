using Newtonsoft.Json;
using System.Collections.Generic;

namespace Aquality.Selenium.Template.Models
{
    public class Project
    {
        [JsonProperty(PropertyName = "projectId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public List<Test> Tests { get; set; }
    }
}

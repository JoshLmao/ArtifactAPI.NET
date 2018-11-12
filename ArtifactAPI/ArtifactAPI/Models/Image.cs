using Newtonsoft.Json;

namespace ArtifactAPI.Models
{
    public class Image
    {
        [JsonProperty("default")]
        public string Default { get; set; }
    }
}

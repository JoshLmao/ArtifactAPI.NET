using Newtonsoft.Json;

namespace ArtifactAPI.Models
{
    public class UrlStage
    {
        [JsonProperty("cdn_root")]
        public string CDNRoot { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }

        [JsonProperty("expire_time")]
        public string ExpireTime { get; set; }
    }
}

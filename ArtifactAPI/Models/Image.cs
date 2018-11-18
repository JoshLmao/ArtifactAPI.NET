using Newtonsoft.Json;

namespace ArtifactAPI.Models
{
    public class Image
    {
        /// <summary>
        /// The default card, is also the English version of the card
        /// </summary>
        [JsonProperty("default")]
        public string Default { get; set; }

        [JsonProperty("german")]
        public string German { get; set; }

        [JsonProperty("french")]
        public string French { get; set; }

        [JsonProperty("italian")]
        public string Italian { get; set; }

        [JsonProperty("koreana")]
        public string Korean { get; set; }

        [JsonProperty("spanish")]
        public string Spanish { get; set; }

        [JsonProperty("schinese")]
        public string StandardChinese { get; set; }

        [JsonProperty("tchinese")]
        public string TraditionalChinese { get; set; }

        [JsonProperty("russian")]
        public string Russian { get; set; }

        [JsonProperty("japanese")]
        public string Japanese { get; set; }

        [JsonProperty("brazilian")]
        public string Brazilian { get; set; }

        [JsonProperty("latam")]
        public string Latam { get; set; }
    }
}

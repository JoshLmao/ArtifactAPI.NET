using ArtifactAPI.Enums;
using Newtonsoft.Json;
using System;

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
        public string ChineseSimplified { get; set; }

        [JsonProperty("tchinese")]
        public string ChineseTraditional { get; set; }

        [JsonProperty("russian")]
        public string Russian { get; set; }

        [JsonProperty("japanese")]
        public string Japanese { get; set; }

        [JsonProperty("brazilian")]
        public string Brazilian { get; set; }

        [JsonProperty("latam")]
        public string Latam { get; set; }

        /// <summary>
        /// Gets the relevant url from the language enum
        /// </summary>
        /// <param name="language">The wanted language</param>
        /// <returns></returns>
        public string GetUrl(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return Default;
                case Language.German:
                    return German;
                case Language.French:
                    return French;
                case Language.Italian:
                    return Italian;
                case Language.Korean:
                    return Korean;
                case Language.Spanish:
                    return Spanish;
                case Language.ChineseSimplified:
                    return ChineseSimplified;
                case Language.ChineseTraditional:
                    return ChineseTraditional;
                case Language.Russian:
                    return Russian;
                case Language.Japanese:
                    return Japanese;
                case Language.Brazilian:
                    return Brazilian;
                case Language.Latam:
                    return Latam;
                default:
                    throw new NotImplementedException("Not able to get language type");
            }
        }
    }
}

using ArtifactAPI.Enums;
using Newtonsoft.Json;
using System;

namespace ArtifactAPI.Models
{
    public class Languages
    {
        [JsonProperty("english")]
        public string English { get; set; }

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
        /// Currently unsupported
        /// </summary>
        [JsonProperty("thai")]
        public string Thai { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("polish")]
        public string Polish { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("danish")]
        public string Danish { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("dutch")]
        public string Dutch { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("finnish")]
        public string Finnish { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("norwegian")]
        public string Norwegian { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("swedish")]
        public string Swedish { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("hungarian")]
        public string Hungarian { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("czech")]
        public string Czech { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("romanian")]
        public string Romanian { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("turkish")]
        public string Turkish { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("bulgarian")]
        public string Bulgarian { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("greek")]
        public string Greek { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("ukrainian")]
        public string Ukranian { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("portuguese")]
        public string Portuguese { get; set; }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        [JsonProperty("vietnamese")]
        public string Vietnamese { get; set; }

        /// <summary>
        /// Gets the relevant translation from the language
        /// </summary>
        /// <param name="language">The wanted translation language</param>
        /// <returns></returns>
        public string GetTranslation(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return English;
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

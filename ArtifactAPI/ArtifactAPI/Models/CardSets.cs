using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI.Models
{
    public class CardSet
    {
        [JsonProperty("card_set")]
        public InnerCardSet Set { get; set; }
    }

    public class InnerCardSet
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("set_info")]
        public Set Info { get; set; }

        [JsonProperty("card_list")]
        List<Card> Cards { get; set; }
    }

    public class Set
    {
        [JsonProperty("set_info")]
        public int Id { get; set; }

        [JsonProperty("pack_item_def")]
        public int PackItem { get; set; }

        [JsonProperty("name")]
        public Languages Names { get; set; }
    }

    public class Languages
    {
        [JsonProperty("english")]
        public string English { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI.Models
{
    public class Card
    {
        [JsonProperty("card_id")]
        public int Id { get; set; }

        [JsonProperty("base_card_id")]
        public int BaseId { get; set; }

        [JsonProperty("card_type")]
        public string Type { get; set; }

        [JsonProperty("card_name")]
        public Languages Names { get; set; }

        [JsonProperty("card_text")]
        public Languages Text { get; set; }

        [JsonProperty("mini_image")]
        public Image MiniImage { get; set; }

        [JsonProperty("large_image")]
        public Image LargeImage { get; set; }

        [JsonProperty("ingame_image")]
        public Image IngameImage { get; set; }
        
        [JsonProperty("mana_cost")]
        public int ManaCost { get; set; }

        [JsonProperty("attack")]
        public int AttackDmg { get; set; }

        [JsonProperty("hit_points")]
        public int HitPoints { get; set; }
    }

    public class Image
    {
        [JsonProperty("default")]
        public string Default { get; set; }
    }
}

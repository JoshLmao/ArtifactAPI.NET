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

        /// <summary>
        /// Type of the card
        /// </summary>
        [JsonProperty("card_type")]
        public string Type { get; set; }

        /// <summary>
        /// All names of the card in all languages
        /// </summary>
        [JsonProperty("card_name")]
        public Languages Names { get; set; }

        /// <summary>
        /// Description text of the card in all languages
        /// </summary>
        [JsonProperty("card_text")]
        public Languages Text { get; set; }

        #region Images
        /// <summary>
        /// The name of the creator of the card art
        /// </summary>
        [JsonProperty("illustrator")]
        public string Illustrator { get; set; }

        /// <summary>
        /// A small square image
        /// </summary>
        [JsonProperty("mini_image")]
        public Image MiniImage { get; set; }

        /// <summary>
        /// The full card image, as would be displayed in game
        /// </summary>
        [JsonProperty("large_image")]
        public Image LargeImage { get; set; }

        /// <summary>
        /// A square sprite image/icon which represents the card icon in game
        /// </summary>
        [JsonProperty("ingame_image")]
        public Image IngameImage { get; set; }
        #endregion

        #region Stats
        /// <summary>
        /// Gold cost to buy this card from the shop
        /// </summary>
        [JsonProperty("gold_cost")]
        public int GoldCost { get; set; }

        /// <summary>
        /// Amount of mana to use the card
        /// </summary>
        [JsonProperty("mana_cost")]
        public int ManaCost { get; set; }

        /// <summary>
        /// Amount of damage the card does
        /// </summary>
        [JsonProperty("attack")]
        public int AttackDmg { get; set; }

        /// <summary>
        /// Amount of armor the card has
        /// </summary>
        [JsonProperty("armor")]
        public int Armor { get; set; }

        /// <summary>
        /// Health of the card
        /// </summary>
        [JsonProperty("hit_points")]
        public int HitPoints { get; set; }

        [JsonProperty("item_def")]
        public int ItemDef { get; set; }
        #endregion

        /// <summary>
        /// The rarity of the card
        /// </summary>
        [JsonProperty("rarity")]
        public string Rarity { get; set; }

        [JsonProperty("is_black")]
        public bool IsBlack { get; set; }

        [JsonProperty("is_green")]
        public bool IsGreen { get; set; }

        [JsonProperty("is_red")]
        public bool IsRed { get; set; }

        [JsonProperty("is_blue")]
        public bool IsBlue { get; set; }

        /// <summary>
        /// List of card id's that relate to this card. For example, their active/passive abilities, etc
        /// </summary>
        [JsonProperty("references")]
        public List<Reference> References { get; set; }
    }

    public class Reference
    {
        [JsonProperty("card_id")]
        public string Id { get; set; }

        [JsonProperty("ref_type")]
        public string Type { get; set; }
    }
}

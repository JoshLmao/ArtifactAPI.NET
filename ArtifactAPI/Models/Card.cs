using ArtifactAPI.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArtifactAPI.Models
{
    public class Card
    {
        [JsonProperty("card_id")]
        public int Id { get; set; }

        [JsonProperty("base_card_id")]
        public int BaseId { get; set; }

        [JsonProperty("item_def")]
        public int ItemDef { get; set; }

        /// <summary>
        /// Type of the card
        /// </summary>
        [JsonProperty("card_type")]
        public CardType Type { get; set; }

        /// <summary>
        /// Subtype of the card
        /// </summary>
        [JsonProperty("sub_type")]
        public CardType SubType { get; set; }

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
        #endregion

        /// <summary>
        /// The rarity of the card
        /// </summary>
        [JsonProperty("rarity")]
        public Rarity Rarity { get; set; }

        /// <summary>
        /// The current color of the card. Can be none for items
        /// </summary>
        public CardColor FactionColor { get; set; } = CardColor.None;

        private bool m_isBlack = false;
        [JsonProperty("is_black"), Obsolete("Use the FactionColor property to get card color")]
        public bool IsBlack
        {
            get { return m_isBlack; }
            set
            {
                m_isBlack = value;
                FactionColor = Enums.CardColor.Black;
            }
        }

        private bool m_isGreen = false;
        [JsonProperty("is_green"), Obsolete("Use the FactionColor property to get card color")]
        public bool IsGreen
        {
            get { return m_isGreen; }
            set
            {
                m_isGreen = value;
                FactionColor = Enums.CardColor.Green;
            }
        }

        private bool m_isRed = false;
        [JsonProperty("is_red"), Obsolete("Use the FactionColor property to get card color")]
        public bool IsRed
        {
            get { return m_isRed; }
            set
            {
                m_isRed = value;
                FactionColor = Enums.CardColor.Red;
            }
        }

        private bool m_isBlue = false;
        [JsonProperty("is_blue"), Obsolete("Use the FactionColor property to get card color")]
        public bool IsBlue
        {
            get { return m_isBlue; }
            set
            {
                m_isBlue = value;
                FactionColor = Enums.CardColor.Blue;
            }
        }

        /// <summary>
        /// List of card id's that relate to this card. For example, their active/passive abilities, etc
        /// </summary>
        [JsonProperty("references")]
        public List<Reference> References { get; set; }
    }

    public class Reference
    {
        [JsonProperty("card_id")]
        public int Id { get; set; }

        [JsonProperty("ref_type")]
        public string Type { get; set; }
    }
}

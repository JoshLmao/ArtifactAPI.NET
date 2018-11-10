using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI.Models
{
    public class DecodedDeck
    {
        /// <summary>
        /// The display name of the deck
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// All heroes within the deck and their turn to be played
        /// </summary>
        public List<DecodedHero> Heroes { get; set; }
        /// <summary>
        /// All cards within the deck and amount of that card
        /// </summary>
        public List<DecodedCard> Cards { get; set; }
    }

    public class DecodedHero
    {
        /// <summary>
        /// ID of the card
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The turn the hero get's played on
        /// </summary>
        public int Turn { get; set; }
    }

    public class DecodedCard
    {
        /// <summary>
        /// The ID of the card
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// How many of the card exists in the deck
        /// </summary>
        public int Count { get; set; }
    }
}

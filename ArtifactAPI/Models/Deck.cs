using System.Collections.Generic;

namespace ArtifactAPI.Models
{
    /// <summary>
    /// An Artifact deck containing the name & all card info
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// The full name of the deck
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// All heroes contained in the deck
        /// </summary>
        public List<HeroCard> Heroes { get; set; }
        /// <summary>
        /// All other cards inside the deck
        /// </summary>
        public List<GenericCard> Cards { get; set; }
    }
}

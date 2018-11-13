namespace ArtifactAPI.Models
{
    public class GenericCard : Card
    {
        /// <summary>
        /// The amount of times this card appears (3 means the deck contains x3 of this card)
        /// </summary>
        public int Count { get; set; }
    }
}

namespace ArtifactAPI.Models
{
    public class SignatureCard : GenericCard
    {
        /// <summary>
        /// The id of the hero card that owns this signature card
        /// </summary>
        public int HeroId { get; set; }
    }
}

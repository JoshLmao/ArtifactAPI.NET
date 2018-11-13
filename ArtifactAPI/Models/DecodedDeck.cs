using System.Collections.Generic;

namespace ArtifactAPI.Models
{
    /// <summary>
    /// A deck decoded from a deck code. Only contains deck id's and required info. SHould be converted into Deck class for full info on cards
    /// </summary>
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

        public DecodedDeck() { }

        /// <summary>
        /// Created a decoded deck from a populated deck
        /// </summary>
        /// <param name="deck">The populated deck</param>
        public DecodedDeck(Deck deck)
        {
            Name = deck.Name;
            if(deck.Heroes != null)
            {
                Heroes = new List<DecodedHero>();
                foreach (HeroCard hero in deck.Heroes)
                {
                    Heroes.Add(new DecodedHero()
                    {
                        Id = hero.Id,
                        Turn = hero.Turn,
                    });
                }
            }
            if(deck.Cards != null)
            {
                Cards = new List<DecodedCard>();
                foreach(GenericCard card in deck.Cards)
                {
                    Cards.Add(new DecodedCard()
                    {
                        Id = card.Id,
                        Count = card.Count,
                    });
                }
            }
        }
    }

    public class DecodedHero : CardId
    {
        /// <summary>
        /// The turn the hero get's played on
        /// </summary>
        public int Turn { get; set; }
    }

    public class DecodedCard : CardId
    {
        /// <summary>
        /// How many of the card exists in the deck
        /// </summary>
        public int Count { get; set; }
    }

    public class CardId
    {
        /// <summary>
        /// The ID of the card
        /// </summary>
        public int Id { get; set; }
    }
}

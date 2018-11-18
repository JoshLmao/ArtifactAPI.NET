using ArtifactAPI.Converters;
using ArtifactAPI.Encoding;
using ArtifactAPI.Enums;
using ArtifactAPI.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtifactAPI
{
    /// <summary>
    /// The entry client for accessing the Artifact API
    /// </summary>
    public class ArtifactClient
    {
        const string ROOT_URL = "https://playartifact.com/";
        const string CDN_ROOT_URL = "https://steamcdn-a.akamaihd.net/";

        private RestClient m_client = null;

        List<Card> m_loadedCards = null;

        public ArtifactClient()
        {
            m_client = new RestClient();
        }

        /// <summary>
        /// Gets all available cards in Artifact
        /// </summary>
        /// <returns>All cards</returns>
        public List<Card> GetAllCards()
        {
            return Task.Run(() => GetAllCardsAsync()).Result;
        }

        /// <summary>
        /// Gets all available cards in Artifact
        /// </summary>
        /// <returns>All cards</returns>
        public async Task<List<Card>> GetAllCardsAsync()
        {
            CardSet setZero = await GetCardSetAsync("00");
            m_loadedCards = setZero.Set.Cards;

            CardSet setOne = await GetCardSetAsync("01");
            m_loadedCards.AddRange(setOne.Set.Cards);

            return m_loadedCards;
        }

        /// <summary>
        /// Returns all cards of a set. Currently, the only sets available are "00" and "01"
        /// </summary>
        /// <param name="cardSetId"></param>
        /// <returns></returns>
        public CardSet GetCardSet(string cardSetId)
        {
            return Task.Run(() => GetCardSetAsync(cardSetId)).Result;
        }

        /// <summary>
        /// Returns all cards of a set asynchronously, the only sets available are "00" and "01"
        /// </summary>
        /// <param name="cardSetId"></param>
        /// <returns></returns>
        public async Task<CardSet> GetCardSetAsync(string cardSetId)
        {
            string stageOneContent = await RequestAsync(ROOT_URL, $"/cardset/{cardSetId}");
            UrlStage stage = null;
            try
            {
                stage = JsonConvert.DeserializeObject<UrlStage>(stageOneContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            string stageTwoContent = await RequestAsync(CDN_ROOT_URL, stage.URL);
            CardSet cardSet = null;
            try
            {
                cardSet = JsonConvert.DeserializeObject<CardSet>(stageTwoContent, new CardToSpecificJsonConverter());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to deserialize - {e}");
            }

            //Set signature cards after all have been loaded
            //ToDo: Set this at the same stage of deserialize, if possible
            cardSet.Set.Cards = await SetSignatureCardsAsync(cardSet.Set.Cards);

            return cardSet;
        }

        private async Task<string> RequestAsync(string baseUrl, string requestUrl)
        {
            m_client.BaseUrl = new Uri(baseUrl);

            RestRequest request = new RestRequest(requestUrl);
            System.Threading.CancellationTokenSource cancelToken = new System.Threading.CancellationTokenSource();
            IRestResponse response = await m_client.ExecuteTaskAsync(request, cancelToken.Token);
            return response.Content;
        }

        /// <summary>
        /// Sorts through cards and sets converts SignatureCards to the correct type
        /// </summary>
        /// <param name="cardSet"></param>
        /// <returns></returns>
        private async Task<List<Card>> SetSignatureCardsAsync(List<Card> cardSet)
        {
            List<Card> modifiedCards = new List<Card>(cardSet);
            await Task.Run(() =>
            {
                foreach (Card gCard in cardSet)
                {
                    if (gCard is HeroCard)
                    {
                        HeroCard heroCard = gCard as HeroCard;
                        if (gCard.References != null)
                        {
                            foreach (Reference r in gCard.References)
                            {
                                if (r.Type == ReferenceType.Included)
                                {
                                    GenericCard g = (GenericCard)modifiedCards.FirstOrDefault(x => x.Id == r.Id);
                                    if (g == null)
                                        continue;

                                    modifiedCards.Remove(g);
                                    SignatureCard sigCard = new SignatureCard()
                                    {
                                        HeroId = gCard.Id, //Set the parent card id of the signature card

                                        Count = 3, //Signature cards always have x3 of theirselves //g.Amount,
                                        Armor = g.Armor,
                                        AttackDmg = g.AttackDmg,
                                        BaseId = g.BaseId,
                                        GoldCost = g.GoldCost,
                                        HitPoints = g.HitPoints,
                                        Id = g.Id,
                                        Illustrator = g.Illustrator,
                                        IngameImage = g.IngameImage,
                                        IsBlack = g.IsBlack,
                                        IsBlue = g.IsBlue,
                                        IsGreen = g.IsGreen,
                                        IsRed = g.IsRed,
                                        FactionColor = g.FactionColor,
                                        ItemDef = g.ItemDef,
                                        LargeImage = g.LargeImage,
                                        ManaCost = g.ManaCost,
                                        MiniImage = g.MiniImage,
                                        Names = g.Names,
                                        Rarity = g.Rarity,
                                        References = g.References,
                                        SubType = g.SubType,
                                        Text = g.Text,
                                        Type = g.Type,
                                    };

                                    modifiedCards.Add(sigCard);
                                }
                            }
                        }
                    }
                }
            });

            return modifiedCards;
        }

        /// <summary>
        /// Encodes a decoded deck into a string for sharing
        /// </summary>
        /// <param name="deck">The decoded deck with hero id's and turns/counts</param>
        /// <returns></returns>
        public string EncodeDeck(DecodedDeck deck)
        {
            if (deck == null)
                return null;

            return DeckEncoder.Encode(deck);
        }

        /// <summary>
        /// Encodes a built deck into a string for sharing
        /// </summary>
        /// <param name="deck">The populated deck</param>
        /// <returns></returns>
        public string EncodeDeck(Deck deck)
        {
            if (deck == null)
                return null;

            DecodedDeck decoded = new DecodedDeck(deck);
            return DeckEncoder.Encode(decoded);
        }

        /// <summary>
        /// Decodes a deck from it's encoded string. Can display deck at https://playartifact.com/d/{url}
        /// </summary>
        /// <param name="encodedDeckString">The base64 encoded string of the deck</param>
        /// <returns></returns>
        public DecodedDeck DecodeDeck(string encodedDeckString)
        {
            if (string.IsNullOrEmpty(encodedDeckString))
                return null;

            return DeckDecoder.Decode(encodedDeckString);
        }

        /// <summary>
        /// Gets a card from a card id
        /// </summary>
        /// <param name="cardId">The id of the card</param>
        /// <returns>The card information and stats</returns>
        public Card GetCard(int cardId)
        {
            return Task.Run(() => GetCardAsync(cardId)).Result;
        }

        /// <summary>
        /// Gets a card from a card id
        /// </summary>
        /// <param name="cardId">The id of the card</param>
        /// <returns>The card information and stats</returns>
        public async Task<Card> GetCardAsync(int cardId)
        {
            if (m_loadedCards == null)
                await GetAllCardsAsync();

            return m_loadedCards.FirstOrDefault(x => x.Id == cardId);
        }

        /// <summary>
        /// Gets a card from the card name
        /// </summary>
        /// <param name="cardName">The English name of the card, casing is ignored</param>
        /// <returns></returns>
        public Card GetCard(string cardName)
        {
            return Task.Run(() => GetCardAsync(cardName)).Result;
        }

        /// <summary>
        /// Gets a card from the card name
        /// </summary>
        /// <param name="cardName">The English name of the card, casing is ignored</param>
        /// <returns></returns>
        public async Task<Card> GetCardAsync(string cardName)
        {
            if (m_loadedCards == null)
                await GetAllCardsAsync();

            return m_loadedCards.FirstOrDefault(x => x.Names.English.ToLower() == cardName.ToLower());
        }

        /// <summary>
        /// Converts a DecodedDeck to return a complete deck with all stats
        /// </summary>
        /// <param name="decodedDeck">A DecodedDeck</param>
        /// <returns>A complete deck with cards and their info</returns>
        public Deck GetCardsFromDecodedDeck(DecodedDeck decodedDeck)
        {
            return Task.Run(() => GetCardsFromDecodedDeckAsync(decodedDeck)).Result;
        }

        /// <summary>
        /// Converts a DecodedDeck to return a complete deck with all stats
        /// </summary>
        /// <param name="decodedDeck">A DecodedDeck</param>
        /// <returns>A complete deck with cards and their info</returns>
        public async Task<Deck> GetCardsFromDecodedDeckAsync(DecodedDeck decodedDeck)
        {
            if (decodedDeck == null)
                return null;

            List<HeroCard> heroCards = new List<HeroCard>();
            List<GenericCard> genericCards = new List<GenericCard>();
            foreach(DecodedHero dHero in decodedDeck.Heroes)
            {
                Card card = await GetCardAsync(dHero.Id);
                if (card == null || !(card is HeroCard))
                    continue;

                HeroCard heroCard = (HeroCard)card;
                heroCard.Turn = dHero.Turn;

                if(heroCard.References != null)
                {
                    foreach (Reference referenceCard in heroCard.References)
                    {
                        Card refCard = await GetCardAsync(referenceCard.Id);
                        if (refCard is SignatureCard)
                            genericCards.Add((SignatureCard)refCard);
                    }
                }

                heroCards.Add(heroCard);
            }

            foreach(DecodedCard dCard in decodedDeck.Cards)
            {
                Card card = await GetCardAsync(dCard.Id);
                if (card == null || !(card is GenericCard))
                    continue;

                GenericCard genericCard = card as GenericCard;
                genericCard.Count = dCard.Count;

                genericCards.Add(genericCard);
            }

            Deck d = new Deck()
            {
                Name = decodedDeck.Name,
                Heroes = heroCards,
                Cards = genericCards,
            };

            return d;
        }

        /// <summary>
        /// Returns the url of the art required from the card's id
        /// </summary>
        /// <param name="cardId">The number id of the card to get art for</param>
        /// <param name="artType">The type of art needed</param>
        /// <param name="language">The language of the image to get</param>
        /// <returns>The url of the image</returns>
        public string GetCardArtUrl(int cardId, ArtType artType, Language language = Language.English)
        {
            return Task.Run(() => GetCardArtUrlAsync(cardId, artType, language)).Result;
        }

        /// <summary>
        /// Returns the url of the art required from the card's id
        /// </summary>
        /// <param name="cardId">The number id of the card to get art for</param>
        /// <param name="artType">The type of art needed</param>
        /// <param name="language">The language of the image to get</param>
        /// <returns>The url of the image</returns>
        public async Task<string> GetCardArtUrlAsync(int cardId, ArtType artType, Language language = Language.English)
        {
            if (m_loadedCards == null)
                await GetAllCardsAsync();

            Card card = m_loadedCards.FirstOrDefault(x => x.Id == cardId);
            if (card == null)
                return null;

            return card.GetArtType(artType).GetUrl(language);
        }

        /// <summary>
        /// Returns the url of the art required from the card name
        /// </summary>
        /// <param name="cardName">The name of the card (in English)</param>
        /// <param name="artType">The type of art needed</param>
        /// <param name="language">The language of the image to get</param>
        /// <returns>The url of the image</returns>
        public string GetCardArtUrl(string cardName, ArtType artType, Language language = Language.English)
        {
            return Task.Run(() => GetCardArtUrlAsync(cardName, artType, language)).Result;
        }

        /// <summary>
        /// Returns the url of the art required from the card name
        /// </summary>
        /// <param name="cardName">The name of the card (in English)</param>
        /// <param name="artType">The type of art needed</param>
        /// <param name="language">The language of the image to get</param>
        /// <returns>The url of the image</returns>
        public async Task<string> GetCardArtUrlAsync(string cardName, ArtType artType, Language language = Language.English)
        {
            Card c = m_loadedCards.FirstOrDefault(x => x.Names.English == cardName);
            if (c == null)
                return null;

            return await GetCardArtUrlAsync(c.Id, artType);
        }

        private string Request(string baseUrl, string requestUrl)
        {
            m_client.BaseUrl = new Uri(baseUrl);

            RestRequest request = new RestRequest(requestUrl);
            IRestResponse response = m_client.Execute(request);
            return response.Content;
        }
    }
}

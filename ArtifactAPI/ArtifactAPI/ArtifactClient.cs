﻿using ArtifactAPI.Encoding;
using ArtifactAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI
{
    public class ArtifactClient
    {
        const string BASE_URL = "https://playartifact.com/";
        const string CDN_ROOT_URL = "https://steamcdn-a.akamaihd.net/";

        private RestClient m_client = null;
        private RestClient m_oClient = null;

        List<Card> m_loadedHeroes = null;

        public ArtifactClient()
        {
            m_client = new RestClient(BASE_URL);
            m_oClient = new RestClient(CDN_ROOT_URL);
        }

        private string Request(RestClient client, string requestUrl)
        {
            RestRequest request = new RestRequest(requestUrl);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        /// <summary>
        /// Returns all cards of the set. Currently, the only sets available are "00" and "01"
        /// </summary>
        /// <param name="cardSetId"></param>
        /// <returns></returns>
        public CardSet GetCardSet(string cardSetId)
        {
            string stageOneContent = Request(m_client, $"/cardset/{cardSetId}");
            UrlStage stage = null;
            try
            {
                stage = JsonConvert.DeserializeObject<UrlStage>(stageOneContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            string stageTwoContent = Request(m_oClient, stage.URL);
            CardSet cardSet = null;
            try
            {
                cardSet = JsonConvert.DeserializeObject<CardSet>(stageTwoContent, new CardToSpecificJsonConverter());
            }
            catch(Exception e)
            {
                Console.WriteLine($"Failed to deserialize - {e}");
            }

            cardSet.Set.Cards = SetSignatureCards(cardSet);

            return cardSet;
        }

        private List<Card> SetSignatureCards(CardSet cardSet)
        {
            List<Card> modifyCards = new List<Card>(cardSet.Set.Cards);

            foreach(Card gCard in cardSet.Set.Cards)
            {
                if(gCard is HeroCard)
                {
                    if(gCard.References != null)
                    {
                        foreach(Reference r in gCard.References)
                        {
                            if(r.Type.ToLower() == "includes")
                            {
                                GenericCard g = (GenericCard)modifyCards.FirstOrDefault(x => x.Id == r.Id);
                                modifyCards.Remove(g);
                                SignatureCard sigCard = new SignatureCard()
                                {
                                    Amount = g.Amount,
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
                                modifyCards.Add(sigCard);
                            }
                        }
                    }
                }
            }

            return modifyCards;
        }

        /// <summary>
        /// Encodes a built deck into a string for sharing
        /// </summary>
        /// <param name="deck">The full deck</param>
        /// <returns>Base8</returns>
        public string EncodeDeck(DecodedDeck deck)
        {
            if (deck == null)
                return null;

            return DeckEncoder.Encode(deck);
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

        public Card GetCard(int id)
        {
            if (m_loadedHeroes == null)
            {
                m_loadedHeroes = GetCardSet("01").Set.Cards;
                var other = GetCardSet("00").Set.Cards;
                m_loadedHeroes.AddRange(other);
            }

            return m_loadedHeroes.FirstOrDefault(x => x.Id == id);
        }

        public Deck GetCardsFromDecodedDeck(DecodedDeck decodedDeck)
        {
            if (decodedDeck == null)
                return null;

            List<HeroCard> heroCards = new List<HeroCard>();
            List<GenericCard> genericCards = new List<GenericCard>();
            foreach(DecodedHero dHero in decodedDeck.Heroes)
            {
                Card card = GetCard(dHero.Id);
                if (card == null || !(card is HeroCard))
                    continue;

                HeroCard heroCard = (HeroCard)card;
                heroCard.Turn = dHero.Turn;

                if(heroCard.References != null)
                {
                    foreach (Reference referenceCard in heroCard.References)
                    {
                        Card refCard = GetCard(referenceCard.Id);
                        if (refCard is SignatureCard)
                            genericCards.Add((SignatureCard)refCard);
                    }
                }

                heroCards.Add(heroCard);
            }

            foreach(DecodedCard dCard in decodedDeck.Cards)
            {
                Card card = GetCard(dCard.Id);
                if (card == null || !(card is GenericCard))
                    continue;

                GenericCard genericCard = card as GenericCard;
                genericCard.Amount = dCard.Count;

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
    }
}

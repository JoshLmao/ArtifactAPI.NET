using System;
using ArtifactAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtifactAPI.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void GetCards()
        {
            ArtifactClient client = new ArtifactClient();
            CardSet cards = client.GetCardSet("01");
            Assert.IsNotNull(cards);
        }

        [TestMethod]
        public void DecodeDeck()
        {
            ArtifactClient client = new ArtifactClient();
            DecodedDeck deck = client.DecodeDeck("ADCJQUQI30zuwEYg2ABeF1Bu94BmWIBTEkLtAKlAZakAYmHh0JsdWUvUmVkIEV4YW1wbGU_");

            Assert.IsNotNull(deck);
            Assert.IsNotNull(deck.Heroes);
            Assert.IsNotNull(deck.Cards);
            Assert.Equals(string.IsNullOrEmpty(deck.Name), false);
        }
    }
}

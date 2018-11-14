using ArtifactAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ArtifactAPI.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void GetAllCards()
        {
            ArtifactClient client = new ArtifactClient();
            List<Card> allCards = client.GetAllCards();

            Assert.IsNotNull(allCards);
        }

        [TestMethod]
        public void GetCardsBySet()
        {
            ArtifactClient client = new ArtifactClient();
            CardSet cards = client.GetCardSet("01");
            Assert.IsNotNull(cards);
        }

        [TestMethod]
        public void FindCardById()
        {
            int ventriloquyCardId = 10418;

            ArtifactClient client = new ArtifactClient();
            Card card = client.GetCard(ventriloquyCardId);

            Assert.IsNotNull(card);
        }

        [TestMethod]
        public void FindCardByName()
        {
            string cardName = "Phantom assassin";

            ArtifactClient client = new ArtifactClient();
            Card c = client.GetCard(cardName);

            Assert.IsNotNull(c);
            Assert.AreEqual(cardName.ToLower(), c.Names.English.ToLower());
        }

        [TestMethod]
        public void GetCardArtUrl()
        {
            ArtifactClient client = new ArtifactClient();
            Card c = client.GetCard("venomancer");

            if (c == null)
                Assert.Fail(); //Failed because GetCard(name) fails

            Enums.ArtType type = Enums.ArtType.Large;
            string idUrl = client.GetCardArtUrl(c.Id, type);
            string nameUrl = client.GetCardArtUrl(c.Names.English, type);

            Assert.IsFalse(string.IsNullOrEmpty(idUrl));
            Assert.IsFalse(string.IsNullOrEmpty(nameUrl));
            Assert.AreEqual(idUrl, nameUrl);
        }
    }
}

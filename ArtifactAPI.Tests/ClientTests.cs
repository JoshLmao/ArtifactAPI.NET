using System;
using ArtifactAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtifactAPI.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void GetCards()
        {
            ArtifactClient client = new ArtifactClient();
            CardSet cards = client.GetCardSet("01");
            Assert.IsNotNull(cards);
        }

        [TestMethod]
        public void FindCardById()
        {
            ArtifactClient client = new ArtifactClient();
            Card card = client.GetCardAsync(10418);

            Assert.IsNotNull(card);
        }
    }
}

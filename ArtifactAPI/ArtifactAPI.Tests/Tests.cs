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
    }
}

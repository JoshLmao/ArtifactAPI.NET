using System;
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
            var cards = client.GetCards("00");
            Assert.AreEqual(cards, null);
        }
    }
}

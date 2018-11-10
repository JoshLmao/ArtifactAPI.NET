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
            CardSet cards = null;
            try
            {
                cards = JsonConvert.DeserializeObject<CardSet>(stageTwoContent);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Failed to deserialize - {e}");
            }

            return cards;
        }

        /// <summary>
        /// Decodes a deck from it's encoded string. Can deplay deck at https://playartifact.com/d/{url}
        /// </summary>
        /// <param name="encodedDeckString">The base64 encoded string of the deck</param>
        /// <returns></returns>
        public DecodedDeck DecodeDeck(string encodedDeckString)
        {
            if (string.IsNullOrEmpty(encodedDeckString))
                return null;

            return DeckDecoder.Decode(encodedDeckString);
        }
    }
}

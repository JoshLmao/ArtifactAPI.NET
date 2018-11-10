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
        const string OTHER_URL = "https://steamcdn-a.akamaihd.net/";

        private RestClient m_client = null;
        private RestClient m_oClient = null;

        public ArtifactClient()
        {
            m_client = new RestClient(BASE_URL);
            m_oClient = new RestClient(OTHER_URL);
        }

        public CardSet GetCards(string cardSetId)
        {
            //00 or 01
            RestRequest request = new RestRequest($"/cardset/{cardSetId}");

            IRestResponse response = m_client.Execute(request);
            string content = response.Content;
            JObject obj = JObject.Parse(content);

            string newUrl = $"{obj["cdn_root"]}{obj["url"]}";

            RestRequest req = new RestRequest(obj["url"].ToString());
            IRestResponse resp = m_oClient.Execute(req);
            string respContent = resp.Content;

            CardSet cards = null;
            try
            {
                cards = JsonConvert.DeserializeObject<CardSet>(respContent);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Failed to deserialize - {e}");
            }

            return cards;
        }
    }
}

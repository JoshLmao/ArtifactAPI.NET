using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI.Models
{
    public class Deck
    {
        public string Name { get; set; }
        public List<HeroCard> Heroes { get; set; }
        public List<GenericCard> Cards {get;set;}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI.Models
{
    public class GenericCard : Card
    {
        /// <summary>
        /// The amount of times this card appears (3 means the deck contains x3 of this card)
        /// </summary>
        public int Amount { get; set; }
    }
}

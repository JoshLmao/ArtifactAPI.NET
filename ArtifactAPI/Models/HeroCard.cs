using System.Linq;
using System.Collections.Generic;

namespace ArtifactAPI.Models
{
    public class HeroCard : Card
    {
        /// <summary>
        /// The number turn that this card will be deployed on
        /// </summary>
        public int Turn { get; set; }
        /// <summary>
        /// The id of the signature card for this hero
        /// </summary>
        public int SignatureCardId { get; set; }

        private List<Reference> m_references = null;
        public override List<Reference> References
        {
            get { return m_references; }
            set
            {
                m_references = value;
                Reference includesRef = m_references.FirstOrDefault(x => x.Type == Enums.ReferenceType.Included);
                if (includesRef != null)
                    SignatureCardId = includesRef.Id;
            }
        }
    }
}

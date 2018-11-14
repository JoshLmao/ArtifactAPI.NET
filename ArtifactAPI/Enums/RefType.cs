using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI.Enums
{
    public enum ReferenceType
    {
        /// <summary>
        /// Cards that are included with the original card. For example, as a signature card
        /// </summary>
        [EnumMember(Value = "includes")]
        Included,
        /// <summary>
        /// Cards that are other references of this card. For example, as it's parent card
        /// </summary>
        [EnumMember(Value = "references")]
        Reference,
        /// <summary>
        /// A cards active ability
        /// </summary>
        [EnumMember(Value = "active_ability")]
        ActiveAbility,
        /// <summary>
        /// A cards passive ability
        /// </summary>
        [EnumMember(Value = "passive_ability")]
        PassiveAbility,
    }
}

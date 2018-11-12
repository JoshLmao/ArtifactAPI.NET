using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI.Enums
{
    /// <summary>
    /// Contains all types, including subtypes. For information on what they do, refer to an Artifact Wiki ;)
    /// </summary>
    public enum CardType
    {
        [EnumMember(Value = "Hero")]
        Hero,
        [EnumMember(Value = "Spell")]
        Spell,
        [EnumMember(Value = "Creep")]
        Creep,
        [EnumMember(Value = "Item")]
        Item,
        [EnumMember(Value = "Ability")]
        Ability,
        [EnumMember(Value = "Passive Ability")]
        PassiveAblity,
        /// <summary>
        /// Card that is either a tower, ancient or rubble (destroyed)
        /// </summary>
        [EnumMember(Value = "Stronghold")]
        Stronghold,
        /// <summary>
        /// A card that display the path of attack
        /// </summary>
        [EnumMember(Value = "Pathing")]
        Pathing,

        [EnumMember(Value = "Deed")]
        Deed,
        [EnumMember(Value = "Improvement")]
        Improvement,
        [EnumMember(Value = "Armor")]
        Armor,
        [EnumMember(Value = "Health")]
        Health,
        [EnumMember(Value = "Weapon")]
        Weapon,
        [EnumMember(Value = "Accessory")]
        Accessory,
        [EnumMember(Value = "Consumable")]
        Consumable,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI.Enums
{
    public enum CardType
    {
        Hero,
        Spell,

        Creep,

        Item,

        Ability,
        PassiveAblity,

        /// <summary>
        /// Is either a tower, ancient or rubble (destroyed)
        /// </summary>
        Stronghold,

        /// <summary>
        /// Cards that display the path of attack
        /// </summary>
        Pathing,
    }
}

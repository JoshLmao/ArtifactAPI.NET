using System.Runtime.Serialization;

namespace ArtifactAPI.Enums
{
    public enum Rarity
    {
        [EnumMember(Value = "common")]
        Common,
        [EnumMember(Value = "uncommon")]
        Uncommon,
        [EnumMember(Value = "rare")]
        Rare
    }
}

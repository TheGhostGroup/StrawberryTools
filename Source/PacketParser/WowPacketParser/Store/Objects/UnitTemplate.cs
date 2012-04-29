using WowPacketParser.Enums;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_template")]
    public class UnitTemplate
    {
        [DBFieldName("name")]
        public string Name;

        [DBFieldName("subname")]
        public string SubName;

        [DBFieldName("IconName")]
        public string IconName;

        [DBFieldName("type_flags")]
        public CreatureTypeFlag TypeFlags;

        [DBFieldName("type_flags2")]
        public uint TypeFlags2;

        [DBFieldName("type")]
        public CreatureType Type;

        [DBFieldName("family")]
        public CreatureFamily Family;

        [DBFieldName("rank")]
        public CreatureRank Rank;

        [DBFieldName("KillCredit1")]
        public uint KillCredit1;

        [DBFieldName("KillCredit2")]
        public uint KillCredit2;

        [DBFieldName("Unknown")]
        public int UnkInt;

        [DBFieldName("PetSpellDataId")]
        public uint PetSpellData;

        [DBFieldName("modelid_", Count = 4)]
        public uint[] DisplayIds;

        [DBFieldName("unk16")]
        public float Modifier1;

        [DBFieldName("unk17")]
        public float Modifier2;

        [DBFieldName("RacialLeader")]
        public bool RacialLeader;

        [DBFieldName("questItem", Count = 6)]
        public uint[] QuestItems;

        [DBFieldName("movementId")]
        public uint MovementId;

        public ClientType Expansion;
    }
}

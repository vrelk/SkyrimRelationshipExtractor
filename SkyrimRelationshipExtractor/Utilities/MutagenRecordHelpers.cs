using Mutagen.Bethesda.Skyrim;
using SkyrimRelationshipExtractor.Models;
using Mutagen.Bethesda.FormKeys.SkyrimSE;

namespace SkyrimRelationshipExtractor.Utilities
{
    internal class MutagenRecordHelpers
    {
        public static string GetActorName(INpcGetter npc)
        {
            if (npc.FormKey == Skyrim.Npc.Player.FormKey)
                return "#PLAYER_NAME#";
            else
                return npc.Name?.String ?? "????";
        }

        /// <summary>
        /// If the AssociationType is "Courting", set the SecondaryTitle to either Boyfriend or Girlfriend.
        /// </summary>
        /// <param name="rel"></param>
        /// <param name="ass"></param>
        public static void FixCourting(ResolvedRelationship rel, ref AssociationRecord ass)
        {
            if(rel.Association?.FormKey != Skyrim.AssociationType.Courting.FormKey)
                return;
            else if (rel.SecondaryActor.Configuration.Flags.HasFlag(NpcConfiguration.Flag.Female))
                ass.SecondaryTitle = "Girlfriend";
            else
                ass.SecondaryTitle = "Boyfriend";
        }

        /// <summary>
        /// Convert a RankType to the int version skyrim uses.
        /// </summary>
        /// <param name="relation"></param>
        /// <returns>-4 to 4</returns>
        public static int RelationshipRankInt(Relationship.RankType relation)
        {
            switch (relation)
            {
                case Relationship.RankType.Lover:
                    return 4;
                case Relationship.RankType.Ally:
                    return 3;
                case Relationship.RankType.Confidant:
                    return 2;
                case Relationship.RankType.Friend:
                    return 1;
                case Relationship.RankType.Acquaintance:
                    return 0;
                case Relationship.RankType.Rival:
                    return -1;
                case Relationship.RankType.Foe:
                    return -2;
                case Relationship.RankType.Enemy:
                    return -3;
                case Relationship.RankType.Archnemesis:
                    return -4;
                default:
                    return 0;
            }
        }
    }
}

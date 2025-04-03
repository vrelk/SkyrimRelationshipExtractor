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
    }
}

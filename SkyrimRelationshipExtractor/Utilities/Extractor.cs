using Mutagen.Bethesda;
using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using SkyrimRelationshipExtractor.Models;

namespace SkyrimRelationshipExtractor.Utilities
{
    internal class Extractor
    {
        public readonly IGameEnvironment<ISkyrimMod, ISkyrimModGetter> env;
        public readonly IEnumerable<IModListingGetter<ISkyrimModGetter>> loadOrder;

        private List<IAssociationTypeGetter> AssociationTypeList;

        public Extractor(string? dataPath = null)
        {
            if (dataPath == null)
                env = GameEnvironment.Typical.Skyrim(SkyrimRelease.SkyrimSE);
            else
            {
                env = GameEnvironment.Typical.Builder<ISkyrimMod, ISkyrimModGetter>(GameRelease.SkyrimSE)
                    .WithTargetDataFolder(dataPath)
                    .Build();
            }
            loadOrder = env.LoadOrder.PriorityOrder.OnlyEnabledAndExisting();

            Console.WriteLine("Skyrim Data Path: " + env.DataFolderPath.Path);
            Console.WriteLine();

            PrintModList();

            // load all association types for lookups
            AssociationTypeList = loadOrder.AssociationType().WinningOverrides().ToList();
        }

        /// <summary>
        /// Print a list of all mods in plugins.txt
        /// </summary>
        private void PrintModList()
        {
            foreach (var item in loadOrder.Reverse())
            {
                if (!item.Enabled)
                    continue;
                else if (!item.ExistsOnDisk)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(item.FileName + " (MISSING)");
                    Console.ResetColor();
                }
                else if (item.Ghosted || !item.Enabled)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(item.FileName + " (Disabled/Ghosted)");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(item.FileName);
                }
            }
            Console.WriteLine();
        }

        public RecordExport ParseRelationships()
        {
            var allRelationships = ResolveAllRelationships();
            List<AssociationRecord> associations = [];
            List<RelationshipRecord> relationships = [];

            foreach (var rel in allRelationships)
            {
                try
                {
#pragma warning disable CS8601, CS8602 // Dereference of a possibly null reference.
                    // this is a named relationship
                    if (rel.Association != null)
                    {
                        AssociationRecord r = new AssociationRecord()
                        {
                            AssType = rel.Association.EditorID,

                            PrimaryActor = rel.PrimaryActor.FormKey.ToString(),
                            PrimaryActorName = MutagenRecordHelpers.GetActorName(rel.PrimaryActor),
                            PrimaryTitle = rel.PrimaryActor.Configuration.Flags.HasFlag(NpcConfiguration.Flag.Female) ? rel.Association.ParentTitle.Female : rel.Association.ParentTitle.Male,

                            SecondaryActor = rel.SecondaryActor.FormKey.ToString(),
                            SecondaryActorName = MutagenRecordHelpers.GetActorName(rel.SecondaryActor),
                            SecondaryTitle = rel.SecondaryActor.Configuration.Flags.HasFlag(NpcConfiguration.Flag.Female) ? rel.Association.Title?.Female ?? "" : rel.Association.Title?.Male ?? "",

                            RelationshipRank = MutagenRecordHelpers.RelationshipRankInt(rel.RelationshipRank),

                            IsDynamic = CheckDynamic(rel)
                        };

                        // Courting records only have the ParentTitle field set, so we want to fill in the other as well
                        MutagenRecordHelpers.FixCourting(rel, ref r);

                        associations.Add(r);
                    }
                    else
                    {
                        var r = new RelationshipRecord()
                        {
                            PrimaryActor = rel.PrimaryActor.FormKey.ToString(),
                            PrimaryActorName = MutagenRecordHelpers.GetActorName(rel.PrimaryActor),

                            SecondaryActor = rel.SecondaryActor.FormKey.ToString(),
                            SecondaryActorName = MutagenRecordHelpers.GetActorName(rel.SecondaryActor),

                            RelationshipRank = MutagenRecordHelpers.RelationshipRankInt(rel.RelationshipRank),

                            IsDynamic = CheckDynamic(rel)
                        };

                        relationships.Add(r);
                    }
#pragma warning restore CS8601, CS8602 // Dereference of a possibly null reference.
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return new RecordExport(associations, relationships);
        }

        // Best guess as to whether this relationship may change during gameplay
        private bool CheckDynamic(ResolvedRelationship relationship)
        {
            // If the player is involved, mark it as dynamic
            if (relationship.PrimaryActor.FormKey == Skyrim.Npc.Player.FormKey || relationship.SecondaryActor.FormKey == Skyrim.Npc.Player.FormKey)
                return true;

            // If one of the actors is a Jarl, mark it as dynamic (player, war, etc)
            else if(CheckRelFaction(relationship, Skyrim.Faction.JobJarlFaction))
                return true;

            else if (CheckRelFaction(relationship, Skyrim.Faction.JobHousecarlFaction))
                return true;

            else if (CheckRelFaction(relationship, Skyrim.Faction.PotentialFollowerFaction))
                return true;

            else if (CheckRelFaction(relationship, Skyrim.Faction.PotentialMarriageFaction))
                return true;


            return false;
        }



        /// <summary>
        /// Iterate through all relationship records in the load order
        /// </summary>
        /// <returns></returns>
        public List<ResolvedRelationship> ResolveAllRelationships()
        {
            var relationships = loadOrder.Relationship().WinningOverrides();

            List<ResolvedRelationship> associationsList = [];

            foreach (var rel in relationships)
            {
                try
                {
                    if (rel.EditorID == null)
                    {
                        continue;
                    }
                    var r = new ResolvedRelationship()
                    {
                        Form = rel.FormKey,

                        PrimaryActor = MutagenResolvers.ResolveNpc(env, rel.Parent),
                        SecondaryActor = MutagenResolvers.ResolveNpc(env, rel.Child),
                        Association = null, // we'll set this later conditionally
                        RelationshipRank = rel.Rank
                    };

                    if (!rel.AssociationType.IsNull)
                    {
                        r.Association = AssociationTypeList.Where(x => x.FormKey == rel.AssociationType.FormKey).FirstOrDefault();
                    }

                    associationsList.Add(r);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(rel.FormKey + ": " + ex.Message);
                }
            }

            return associationsList;
        }

        /// <summary>
        /// Same as IsInFaction, but checks both actors.
        /// </summary>
        /// <param name="rel"></param>
        /// <param name="faction"></param>
        /// <returns></returns>
        private bool CheckRelFaction(ResolvedRelationship rel, FormLink<IFactionGetter> faction)
        {
            return IsInFaction(rel.PrimaryActor, faction) || IsInFaction(rel.SecondaryActor, faction);
        }

        /// <summary>
        /// Check if the given npc is in the given faction.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="faction"></param>
        /// <returns></returns>
        private bool IsInFaction(INpcGetter npc, FormLink<IFactionGetter> faction)
        {
            return npc.Factions.Any(x => x.Faction == faction);
        }
    }
}

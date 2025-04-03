using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;

namespace SkyrimRelationshipExtractor.Models
{
    /// <summary>
    /// 
    /// </summary>
    internal class ResolvedRelationship
    {
        public required FormKey Form { get; set; }

        /// <summary>
        /// Given ParentChild, this would be the parent
        /// </summary>
        public required INpcGetter PrimaryActor { get; set; }

        /// <summary>
        /// Given ParentChild, this would be the child
        /// </summary>
        public required INpcGetter SecondaryActor { get; set; }

        /// <summary>
        /// If this is set, use AssociationRecord, otherwise use RelationshipRecord
        /// </summary>
        public IAssociationTypeGetter? Association { get; set; } = null;

        /// <summary>
        /// This can be read as an integer
        /// </summary>
        public required Relationship.RankType RelationshipRank { get; set; }
    }
}

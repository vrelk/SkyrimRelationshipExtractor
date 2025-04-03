using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrimRelationshipExtractor.Models
{
    /// <summary>
    /// When only the relationship rank is set between two actors
    /// </summary>
    internal struct RelationshipRecord
    {
        /// <summary>
        /// FormKey of the primary actor
        /// </summary>
        public string PrimaryActor;

        /// <summary>
        /// Name of the primary actor
        /// </summary>
        public string PrimaryActorName;

        /// <summary>
        /// FormKey of the secondary actor
        /// </summary>
        public string SecondaryActor;

        /// <summary>
        /// Name of the secondary actor
        /// </summary>
        public string SecondaryActorName;

        /// <summary>
        /// The integer value for relationship rank.
        /// </summary>
        /// <see cref="https://ck.uesp.net/wiki/GetRelationshipRank_-_Actor"/>
        public int RelationshipRank;

        /// <summary>
        /// Best guess as to whether this may change during gameplay
        /// </summary>
        public bool IsDynamic;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrimRelationshipExtractor.Models
{
    /// <summary>
    /// When the relationship between two actors has an actual title (ex. mother/daughter)
    /// </summary>
    internal struct AssociationRecord
    {
        /// <summary>
        /// EditorID of the AssociationType record
        /// </summary>
        public string AssType;

        /// <summary>
        /// FormKey of the primary actor
        /// </summary>
        public string PrimaryActor;

        /// <summary>
        /// Name of the primary actor
        /// </summary>
        public string PrimaryActorName;

        /// <summary>
        /// Association position title for the primary actor
        /// </summary>
        public string PrimaryTitle;

        /// <summary>
        /// FormKey of the secondary actor
        /// </summary>
        public string SecondaryActor;

        /// <summary>
        /// Name of the secondary actor
        /// </summary>
        public string SecondaryActorName;

        /// <summary>
        /// Association title of the secondary actor
        /// </summary>
        public string SecondaryTitle;

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

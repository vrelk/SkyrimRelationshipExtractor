using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SkyrimRelationshipExtractor.Models
{
    /// <summary>
    /// This is just used for json serialization.
    /// </summary>
    internal struct RecordExport
    {
        public List<AssociationRecord> Associations;
        public List<RelationshipRecord> Relations;

        public RecordExport(List<AssociationRecord> associationRecords, List<RelationshipRecord> relationshipRecords)
        {
            Associations = associationRecords;
            Relations = relationshipRecords;
        }

        public readonly string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}

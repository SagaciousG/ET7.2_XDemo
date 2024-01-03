using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class UnitNameComponent : Entity, IAwake
    {
        public List<string> UsingNames = new List<string>();

        [BsonIgnore]
        public HashSet<string> UsingNamesSet = new HashSet<string>();

        [BsonIgnore]
        public HashSet<string> UnusedNames = new HashSet<string>();
        
        [BsonIgnore]
        public HashSet<string> DefinedNames = new HashSet<string>();
    }
}
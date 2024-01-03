using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(Profession))]
    public class ArmsComponent : Entity, IAwake, ISerializeToEntity
    {
        [BsonIgnore]
        public Dictionary<EquipmentHole, ArmsItem> HoleItem { get; set; } = new();
    }
    
  
}
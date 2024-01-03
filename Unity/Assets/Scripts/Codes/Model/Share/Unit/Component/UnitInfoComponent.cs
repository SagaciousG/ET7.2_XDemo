using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class UnitInfoComponent : Entity, ISerializeToEntity, IAwake
    {
        //当前正处于的GateWay中
        [BsonIgnore]
        public int BeInGateWay { get; set; }
        
        [BsonIgnore] public long LoopSave { get; set; }
        
        [BsonIgnore]
        public bool IsOnline { get; set; }
    }
}
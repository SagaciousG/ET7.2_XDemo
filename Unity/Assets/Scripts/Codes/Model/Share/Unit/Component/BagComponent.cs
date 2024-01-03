using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class BagComponent : Entity, ISerializeToEntity, IAwake, IDeserialize
    {
        [BsonIgnore]
        public MultiMap<int, BagItem> BagItems { get; set; } = new();
    }

    public struct Event_OnBagItemChange
    {
        public List<BagItemProto> Items;
        public Unit Unit;
    }
}
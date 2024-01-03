using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf(typeof(ArmsComponent))]
    public class ArmsItem : Entity, IAwake<EquipmentHole>, ISerializeToEntity
    {
        public EquipmentHole Hole { get; set; }
    }
}
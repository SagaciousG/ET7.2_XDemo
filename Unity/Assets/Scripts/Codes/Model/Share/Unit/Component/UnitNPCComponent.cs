namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class UnitNPCComponent : Entity, ISerializeToEntity, IAwake
    {
        public int NPCID { get; set; }
        public long ImageFor { get; set; }
    }
}
namespace ET
{
    [ComponentOf(typeof(BagItem))]
    public class BagItemArmComponent : Entity, ISerializeToEntity, IAwake
    {
        public bool Equipped { get; set; }
    }
}
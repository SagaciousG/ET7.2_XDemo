
namespace ET.Server
{
    [ChildOf(typeof(Player))]
    public class PlayerUnit : Entity, IAwake, ISerializeToEntity
    {
        public string Name { get; set; }
        public int UnitShow { get; set; }
        public int Level { get; set; }
    }
}
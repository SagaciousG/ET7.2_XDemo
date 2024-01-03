namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class GateComponent : Entity, IAwake, IDestroy
    {
        public long MapActorID { get; set; }
    }
}
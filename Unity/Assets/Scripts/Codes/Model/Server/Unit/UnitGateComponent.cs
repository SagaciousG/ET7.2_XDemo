namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class UnitGateComponent : Entity, IAwake
    {
        public long GateSessionActorID { get; set; }
    }
}
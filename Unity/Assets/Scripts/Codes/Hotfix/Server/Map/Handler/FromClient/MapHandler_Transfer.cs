
namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class MapHandler_Transfer : AMActorLocationHandler<Unit, TransferMapALMessage>
    {
        protected override async ETTask Run(Unit unit, TransferMapALMessage message)
        {
            if (unit.GetComponent<UnitInfoComponent>().BeInGateWay != message.FromGateWay)
                return;
            var cfg = GateWayConfigCategory.Instance.Get(message.FromGateWay);
            var toCfg = GateWayConfigCategory.Instance.Get(cfg.ToGateWay);
            unit.GetComponent<UnitInfoComponent>().BeInGateWay = toCfg.Id;
            TransferHelper.Transfer(unit, toCfg.Map, toCfg.Position.ToFloat3(), toCfg.Rotation.ToQuaternion()).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}
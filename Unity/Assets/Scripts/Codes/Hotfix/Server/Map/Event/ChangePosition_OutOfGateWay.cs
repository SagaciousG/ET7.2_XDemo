using ET.EventType;
using Unity.Mathematics;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class ChangePosition_OutOfGateWay : AEvent<ChangePosition>
    {
        protected override async ETTask Run(Scene scene, ChangePosition a)
        {
            var map = scene.GetComponent<MapComponent>().GetMap(a.Unit.Map);
            // var mapUnitComponent = map.GetComponent<MapUnitComponent>();
            // foreach (var entity in gateWayComponent.Children.Values)
            // {
            //     var gateWay = (GateWay)entity;
            //     if (a.Unit.Position.Distance(gateWay.Position) <= 1)
            //     {
            //         if(a.Unit.BeInGateWay == gateWay.ConfigId)
            //             return;
            //         a.Unit.BeInGateWay = gateWay.ConfigId;
            //         return;
            //     }
            // }
            //
            // a.Unit.BeInGateWay = 0;
            await ETTask.CompletedTask;
        }
    }
}
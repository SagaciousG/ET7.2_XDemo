using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class ChangePosition_GateWayDetecte : AEvent<ChangePosition>
    {
        protected override async ETTask Run(Scene scene, ChangePosition a)
        {
            if (a.Unit.Id != scene.ClientScene().GetComponent<PlayerComponent>().MyUnitId)
                return;
            // var gateWayComponent = scene.GetComponent<GateWayComponent>();
            // foreach (var entity in gateWayComponent.Children.Values)
            // {
            //     var gateWay = (GateWay)entity;
            //     if (Vector3.Distance(a.Unit.Position, gateWay.Position) <= 1)
            //     {
            //         if(a.Unit.BeInGateWay == gateWay.ConfigId)
            //             return;
            //         a.Unit.BeInGateWay = gateWay.ConfigId;
            //         SessionHelper.Send(scene.ClientScene(), new TransferMapALMessage(){FromGateWay = gateWay.ConfigId});
            //         return;
            //     }
            // }
            //
            // a.Unit.BeInGateWay = 0;

            await ETTask.CompletedTask;
        }
    }
}
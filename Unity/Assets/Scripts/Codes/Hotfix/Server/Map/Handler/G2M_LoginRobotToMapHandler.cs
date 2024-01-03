using System;
using Unity.Mathematics;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class G2M_LoginRobotToMapHandler : AMActorRpcHandler<Scene, G2M_LoginRobotToMapARequest, G2M_LoginRobotToMapAResponse>
    {
        protected override async ETTask Run(Scene scene, G2M_LoginRobotToMapARequest request, G2M_LoginRobotToMapAResponse response, Action reply)
        {
            var dbComponent = scene.GetComponent<DBComponent>();
            var unit = await dbComponent.Query<Unit>(request.Robot.UnitId);
            if (unit == null) // 新建
            {
                unit = UnitFactory.CreateNewUnit(scene, request.Robot);
            }
            else
            {
                scene.GetComponent<UnitComponent>().Add(unit);
            }
            
            unit.AddComponent<MoveComponent>();
            unit.AddComponent<TeamComponent>();
            unit.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnitMessageDispatcher);
            unit.AddComponent<AOIEntity, int, float3>(9 * 1000, unit.Position);
            unit.AddComponent<MeetMonsterComponent>();
            unit.AddComponent<NumericComponent>();
            
            ET.UnitHelper.SetUnitLevel(unit, unit.Level);
            unit.AddLocation().Coroutine();
            
            await TransferHelper.RobotTransfer(unit, unit.Map, unit.Position, unit.Rotation);
            reply();
            await ETTask.CompletedTask;
        }
    }
}
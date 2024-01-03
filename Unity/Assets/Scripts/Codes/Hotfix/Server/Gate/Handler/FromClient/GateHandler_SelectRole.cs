using System;

namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class GateHandler_SelectRole : AMRpcHandler<SelectRoleRequest, SelectRoleResponse>
    {
        protected override async ETTask Run(Session session, SelectRoleRequest request, SelectRoleResponse response, Action reply)
        {
            var myPlayer = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            myPlayer.UnitId = request.UnitId;
            myPlayer.Session = session;
            var playerUnit = myPlayer.GetChild<PlayerUnit>(request.UnitId);
            var login = await MessageHelper.CallActor(session.DomainScene().GetComponent<GateComponent>().MapActorID,
                new G2M_LoginInMapARequest() { 
                    Unit = new SimpleUnit()
                    {
                        UnitId = request.UnitId,
                        Level = playerUnit.Level,
                        Name = playerUnit.Name,
                        UnitShow = playerUnit.UnitShow,
                        UnitType = (int) UnitType.Player
                    },
                    GateSessionActorID = session.InstanceId
                });
            reply();
            await ETTask.CompletedTask;
        }
    }
}
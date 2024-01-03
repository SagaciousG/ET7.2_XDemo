using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class GateHandler_CreateUnit : AMRpcHandler<CreateRoleRequest, CreateRoleResponse>
    {
        protected override async ETTask Run(Session session, CreateRoleRequest request, CreateRoleResponse response, Action reply)
        {
            var unitNameComponent = session.DomainScene().GetComponent<UnitNameComponent>();
            if (unitNameComponent.IsUsing(request.Name))
            {
                response.Error = ErrorCode.ERR_UnitNameUsed;
                reply();
            }
            unitNameComponent.Use(request.Name);
            unitNameComponent.Save().Coroutine();
            
            var myPlayer = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            var unitId = IdGenerater.Instance.GenerateUnitId(session.DomainZone());
            var playerUnit = myPlayer.AddChildWithId<PlayerUnit>(unitId);
            playerUnit.UnitShow = request.ShowID;
            playerUnit.Level = 1;
            playerUnit.Name = request.Name;
            
            Log.Console($"[Gate{session.DomainZone()}]Create Unit {myPlayer.Account} {unitId}");
            var dbComponent = session.DomainScene().GetComponent<DBComponent>();
            await dbComponent.Save(myPlayer);
            reply();
            await ETTask.CompletedTask;
        }
    }
}
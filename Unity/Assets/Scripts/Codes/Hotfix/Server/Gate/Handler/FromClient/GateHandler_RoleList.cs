using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class GateHandler_RoleList : AMRpcHandler<RoleListRequest, RoleListResponse>
    {
        protected override async ETTask Run(Session session, RoleListRequest request, RoleListResponse response, Action reply)
        {
            var playerId = session.GetComponent<SessionPlayerComponent>().PlayerId;
            var playerComponent = session.DomainScene().GetComponent<PlayerComponent>();
            var player = playerComponent.Get(playerId);
            if (player.GetChildren<PlayerUnit>().Length != 0)
            {
                response.Units ??= new List<SimpleUnit>();
                foreach (var playerUnit in player.GetChildren<PlayerUnit>())
                {
                    response.Units.Add(new SimpleUnit(){
                        UnitId = playerUnit.Id,
                        Level = playerUnit.Level,
                        Name = playerUnit.Name,
                        UnitShow = playerUnit.UnitShow
                    });
                }
            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}
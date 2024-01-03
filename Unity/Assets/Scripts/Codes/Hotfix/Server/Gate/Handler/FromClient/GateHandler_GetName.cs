using System;
using System.Linq;

namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class GateHandler_GetName : AMRpcHandler<GetNameRequest, GetNameResponse>
    {
        protected override async ETTask Run(Session session, GetNameRequest request, GetNameResponse response, Action reply)
        {
            var unitNameComponent = session.DomainScene().GetComponent<UnitNameComponent>();
            response.Name = unitNameComponent.RandomGet();
            reply();
            await ETTask.CompletedTask;
        }
    }
}
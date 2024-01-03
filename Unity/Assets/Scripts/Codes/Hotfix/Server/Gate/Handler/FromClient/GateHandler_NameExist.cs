using System;

namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class GateHandler_NameExist : AMRpcHandler<NameExistRequest, NameExistResponse>
    {
        protected override async ETTask Run(Session session, NameExistRequest request, NameExistResponse response, Action reply)
        {
            var unitNameComponent = session.DomainScene().GetComponent<UnitNameComponent>();

            response.Error = unitNameComponent.IsUsing(request.Name)? ErrorCode.ERR_UnitNameUsed : 0;
            reply();
            await ETTask.CompletedTask;
        }
    }
}
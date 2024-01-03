using System;

namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class GateHandler_Interact : AMRpcHandler<InteractToRequest, InteractToResponse>
    {
        protected override async ETTask Run(Session session, InteractToRequest request, InteractToResponse response, Action reply)
        {
            var myPlayer = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            reply();

            await ETTask.CompletedTask;
        }
    }
}
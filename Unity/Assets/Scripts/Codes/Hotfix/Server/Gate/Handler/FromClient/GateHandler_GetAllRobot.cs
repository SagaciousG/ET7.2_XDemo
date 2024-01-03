using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class GateHandler_GetAllRobot : AMRpcHandler<GetAllRobotRequest, GetAllRobotResponse>
    {
        protected override async ETTask Run(Session session, GetAllRobotRequest request, GetAllRobotResponse response, Action reply)
        {
            var robotManagerComponent = session.DomainScene().GetComponent<RobotManagerComponent>();
            response.AllRobot = new();
            foreach (var item in robotManagerComponent.GetChildren<Robot>())
            {
                response.AllRobot.Add(item);
            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}
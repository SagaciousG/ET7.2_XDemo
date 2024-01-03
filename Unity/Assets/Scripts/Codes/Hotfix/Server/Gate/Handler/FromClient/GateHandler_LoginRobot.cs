using System;
using System.Linq;

namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class GateHandler_LoginRobot : AMRpcHandler<LoginRobotRequest, LoginRobotResponse>
    {
        protected override async ETTask Run(Session session, LoginRobotRequest request, LoginRobotResponse response, Action reply)
        {
            var robotManagerComponent = session.DomainScene().GetComponent<RobotManagerComponent>();
            switch (request.Account)
            {
                case -1:
                    robotManagerComponent.NewRobot().Coroutine();
                    break;
                case 0:
                {
                    var result = robotManagerComponent.GetChildren<Robot>().Where(a => a.Online == 0).ToList();
                    var random = new Random();
                    random.BreakRank(result);
                    if (result.Count > 0)
                    {
                        robotManagerComponent.LoginRobot(result[1].Id).Coroutine();
                    }
                    break;
                }
                default:
                    robotManagerComponent.LoginRobot(request.Account).Coroutine();
                    break;
            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}
using System.Collections.Generic;

namespace ET.Server
{
	[ActorMessageHandler(SceneType.Map)]
	public class C2M_StopHandler : AMActorLocationHandler<Unit, StopMoveALMessage>
	{
		protected override async ETTask Run(Unit unit, StopMoveALMessage message)
		{
			unit.Stop(0);
			await ETTask.CompletedTask;
		}
	}
}
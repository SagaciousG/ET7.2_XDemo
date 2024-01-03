using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
	[ActorMessageHandler(SceneType.Map)]
	public class MapHandler_FindPath : AMActorLocationHandler<Unit, FindPathALMessage>
	{
		protected override async ETTask Run(Unit unit, FindPathALMessage message)
		{
			float3 target = message.Position;

			unit.FindPathMoveToAsync(target).Coroutine();
			unit.SetDirty();
			await ETTask.CompletedTask;
		}
	}
}
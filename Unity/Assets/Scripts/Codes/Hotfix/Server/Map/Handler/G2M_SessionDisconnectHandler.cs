

using System.Collections.Generic;

namespace ET.Server
{
	[ActorMessageHandler(SceneType.Map)]
	public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Unit, G2M_SessionDisconnectALMessage>
	{
		protected override async ETTask Run(Unit unit, G2M_SessionDisconnectALMessage message)
		{
			Log.Console($"[Map{unit.DomainZone()}] Unit Disconnect {unit.Id}");
			unit.GetComponent<UnitInfoComponent>().IsOnline = false;
			MessageHelper.Broadcast(unit, new UnitDisconnectAMessage(){UnitID = unit.Id});

			unit.GetParent<UnitComponent>().Remove(unit.Id);
			await ETTask.CompletedTask;
		}
	}
}
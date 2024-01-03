

namespace ET.Server
{
	public static class SessionPlayerComponentSystem
	{
		public class SessionPlayerComponentDestroySystem: DestroySystem<SessionPlayerComponent>
		{
			protected override void Destroy(SessionPlayerComponent self)
			{
				// 发送断线消息
				Log.Console($"[Gate] Player Disconnect {self.GetMyPlayer().Account} {self.GetMyPlayer().UnitId}");
				MessageHelper.SendToLocationActor(self.GetMyPlayer().UnitId, new G2M_SessionDisconnectALMessage());
				self.DomainScene().GetComponent<PlayerComponent>()?.Remove(self.PlayerId);
			}
		}

		public static Player GetMyPlayer(this SessionPlayerComponent self)
		{
			return self.DomainScene().GetComponent<PlayerComponent>().Get(self.PlayerId);
		}
	}
}

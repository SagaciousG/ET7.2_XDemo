
namespace ET.Client
{
	[MessageHandler(SceneType.Client)]
	public class MoveStopHandler : AMHandler<M2C_StopAMessage>
	{
		protected override async ETTask Run(Session session, M2C_StopAMessage message)
		{
			Unit unit = session.DomainScene().CurrentScene().GetComponent<UnitComponent>().Get(message.Id);
			if (unit == null)
			{
				return;
			}

			MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
			moveComponent.Stop();
			unit.Position = message.Position;
			unit.Rotation = message.Rotation;
			unit.GetComponent<ObjectWait>()?.Notify(new Wait_UnitStop() {Error = message.Error});
			EventSystem.Instance.Publish(session.ClientScene().CurrentScene(), new MoveStop() {Unit = unit});
			await ETTask.CompletedTask;
		}
	}
}

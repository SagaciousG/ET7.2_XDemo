namespace ET.Client
{
	[MessageHandler(SceneType.Client)]
	public class RemoveUnitsHandler : AMHandler<RemoveUnitsAMessage>
	{
		protected override async ETTask Run(Session session, RemoveUnitsAMessage message)
		{	
			UnitComponent unitComponent = session.DomainScene().CurrentScene()?.GetComponent<UnitComponent>();
			if (unitComponent == null)
			{
				return;
			}
			foreach (long unitId in message.Units)
			{
				unitComponent.Remove(unitId);
			}

			await ETTask.CompletedTask;
		}
	}
}

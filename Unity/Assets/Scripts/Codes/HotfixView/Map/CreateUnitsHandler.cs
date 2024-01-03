namespace ET.Client
{
	[MessageHandler(SceneType.Client)]
	public class CreateUnitsHandler : AMHandler<CreateUnitsAMessage>
	{
		protected override async ETTask Run(Session session, CreateUnitsAMessage message)
		{
			Scene currentScene = session.DomainScene().CurrentScene();
			var unitySceneComponent = currentScene.GetComponent<UnitySceneComponent>();
			await currentScene.Wait(WaiterKey.CreateMyUnit);
			
			UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
			
			foreach (UnitProto unitInfo in message.Units)
			{
				if (unitySceneComponent.MapID != unitInfo.Map)
				{
					Log.Error($"创建Unit所在地图与当前地图不匹配，{unitInfo.SimpleUnit.UnitId} map={unitInfo.Map}, cur={unitySceneComponent.MapID}");
					continue;
				}
				if (unitComponent.Get(unitInfo.SimpleUnit.UnitId) != null)
				{
					continue;
				}
				Unit unit = UnitFactory.Create(currentScene, unitInfo);
			}
			await ETTask.CompletedTask;
		}
	}
}

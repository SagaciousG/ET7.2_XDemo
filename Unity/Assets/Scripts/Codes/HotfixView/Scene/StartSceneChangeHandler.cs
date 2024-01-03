namespace ET.Client
{
	[MessageHandler(SceneType.Client)]
	public class StartSceneChangeHandler : AMHandler<StartSceneChangeAMessage>
	{
		protected override async ETTask Run(Session session, StartSceneChangeAMessage message)
		{
			await SceneChangeHelper.SceneChangeTo(session.ClientScene(), message.MapId, message.MapActorId);
		}
	}
}

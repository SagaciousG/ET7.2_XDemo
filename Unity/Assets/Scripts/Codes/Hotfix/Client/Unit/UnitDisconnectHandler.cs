namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class UnitDisconnectHandler : AMHandler<UnitDisconnectAMessage>
    {
        protected override async ETTask Run(Session session, UnitDisconnectAMessage message)
        {
            var unit = await UnitHelper.GetMyUnitFromClientScene(session.ClientScene());
            unit.GetComponent<UnitInfoComponent>().IsOnline = false;
            await ETTask.CompletedTask;
        }
    }

}
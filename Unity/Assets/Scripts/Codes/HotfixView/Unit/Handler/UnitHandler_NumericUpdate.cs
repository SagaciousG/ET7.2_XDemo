namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class UnitHandler_NumericUpdate : AMHandler<UnitNumericUpdateAMessage>
    {
        protected override async ETTask Run(Session session, UnitNumericUpdateAMessage message)
        {
            var myUnit = await UnitHelper.GetMyUnitFromClientScene(session.ClientScene());
            myUnit.GetComponent<NumericComponent>().NumericDic.Clear();            
            myUnit.GetComponent<NumericComponent>().NumericDic.AddRange(message.Numeric);   
            SignalHelper.Fire(myUnit.DomainScene(), SignalKey.UI_NumericChanged);
            await ETTask.CompletedTask;
        }
    }
}
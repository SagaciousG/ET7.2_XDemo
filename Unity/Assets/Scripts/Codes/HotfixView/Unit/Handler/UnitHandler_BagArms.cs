namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class UnitHandler_BagArms : AMHandler<UnitBagArmsAMessage>
    {
        protected override async ETTask Run(Session session, UnitBagArmsAMessage message)
        {
            var myUnit = await UnitHelper.GetMyUnitFromClientScene(session.ClientScene());
            var bagComponent = myUnit.GetComponent<BagComponent>();
            if (message.Arms == null)
                return;
            foreach (var itemProto in message.Arms)
            {
                var bagItem = bagComponent.GetByUID(itemProto.UID);
                var bagItemArmComponent = bagItem.GetComponent<BagItemArmComponent>();
                bagItemArmComponent.Equipped = itemProto.Equipped == 1;
            }

            EventSystem.Instance.PublishAsync(session.ClientScene(), new Event_OnBagUpdate()).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}
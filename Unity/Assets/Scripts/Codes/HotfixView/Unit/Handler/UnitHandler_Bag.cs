namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class UnitHandler_Bag : AMHandler<UnitBagAMessage>
    {
        protected override async ETTask Run(Session session, UnitBagAMessage message)
        {
            var myUnit = await UnitHelper.GetMyUnitFromClientScene(session.ClientScene());
            var bagComponent = myUnit.GetComponent<BagComponent>();
            if (message.Items == null)
                return;
            foreach (var itemProto in message.Items)
            {
                var change = bagComponent.Update(itemProto);
                
                switch (itemProto.ID)
                {
                    case (int)MoneyType.Exp:
                        OnExp(myUnit);
                        break;
                }
                SignalHelper.Fire(session.ClientScene(), SignalKey.UI_BagItemChanged_ID_Num, itemProto.ID, change);
            }

            EventSystem.Instance.PublishAsync(session.ClientScene(), new Event_OnBagUpdate()).Coroutine();
            await ETTask.CompletedTask;
        }
        
        private void OnExp(Unit unit)
        {
            var oldLv = unit.Level;
            ET.UnitHelper.RefreshLevel(unit);
            if (oldLv != unit.Level)
            {
                ET.UnitHelper.SetUnitLevel(unit, unit.Level);
                EventSystem.Instance.PublishAsync(unit.DomainScene(), new UnitLevelChange(){OldLevel = oldLv, Unit = unit}).Coroutine();
            }
        }
    }
}
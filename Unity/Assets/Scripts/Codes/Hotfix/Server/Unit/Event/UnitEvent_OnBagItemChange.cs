namespace ET.Server
{
    [Event(SceneType.Map)]
    public class UnitEvent_OnBagItemChange : AEvent<Event_OnBagItemChange>
    {
        protected override async ETTask Run(Scene scene, Event_OnBagItemChange a)
        {
            a.Unit.GetComponent<BagComponent>().SendBagItemChange(a.Items);
            foreach (var itemProto in a.Items)
            {
                switch (itemProto.ID)
                {
                    case (int)MoneyType.Exp:
                        OnExp(a.Unit);
                        break;
                }
            }
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
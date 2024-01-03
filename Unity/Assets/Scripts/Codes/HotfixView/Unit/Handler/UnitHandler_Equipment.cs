namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class UnitHandler_Equipment : AMHandler<UnitEquipmentUpdateAMessage>
    {
        protected override async ETTask Run(Session session, UnitEquipmentUpdateAMessage message)
        {
            if (message.Equips == null)
                return;
            var unit = await UnitHelper.GetMyUnitFromClientScene(session.ClientScene());
            foreach (var equipmentProto in message.Equips)
            {
                var profession = unit.GetProfession((ProfessionNum)equipmentProto.ProfessionNum);
                var armsComponent = profession.GetComponent<ArmsComponent>();
                if (equipmentProto.EquipUp == 1)
                {
                    armsComponent.EquipUp(equipmentProto.UID, (EquipmentHole)equipmentProto.Hole);
                }
                else
                {
                    armsComponent.HoleItem.Remove((EquipmentHole)equipmentProto.Hole);
                    armsComponent.RemoveChild(equipmentProto.UID);
                }

                SignalHelper.Fire(unit.DomainScene(), SignalKey.UI_EquipmentChanged_Hole, equipmentProto.Hole);
            }
            EventSystem.Instance.PublishAsync(unit.DomainScene(), new UnitProfessionUpdate(){Unit = unit}).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}
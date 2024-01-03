namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class UnitHandler_Profession : AMHandler<UnitProfessionAMessage>
    {
        protected override async ETTask Run(Session session, UnitProfessionAMessage message)
        {
            var unit = await UnitHelper.GetMyUnitFromClientScene(session.ClientScene());
            foreach (var professionProto in message.Professions)
            {
                var profession = unit.GetChild<Profession>(professionProto.UID);
                if (profession == null)
                {
                    profession = unit.AddChildWithId<Profession, ProfessionNum>(professionProto.UID,
                        (ProfessionNum)professionProto.Num);
                    profession.AddComponent<ArmsComponent>();
                }
                
            }
            EventSystem.Instance.PublishAsync(unit.DomainScene(), new UnitProfessionUpdate(){Unit = unit}).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}
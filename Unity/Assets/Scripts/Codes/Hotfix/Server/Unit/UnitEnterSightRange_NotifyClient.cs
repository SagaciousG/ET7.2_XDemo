namespace ET.Server
{
    // 进入视野通知
    [Event(SceneType.Map)]
    public class UnitEnterSightRange_NotifyClient: AEvent<Event_UnitEnterSightRange>
    {
        protected override async ETTask Run(Scene scene, Event_UnitEnterSightRange args)
        {
            AOIEntity a = args.A;
            AOIEntity b = args.B;
            if (a.Id == b.Id)
            {
                return;
            }

            Unit ua = a.GetParent<Unit>();
            // if (ua.Type != UnitType.Player)
            // {
                // return;
            // }

            Unit ub = b.GetParent<Unit>();

            MessageHelper.NoticeUnitAdd(ua, ub);
            
            await ETTask.CompletedTask;
        }
    }
}
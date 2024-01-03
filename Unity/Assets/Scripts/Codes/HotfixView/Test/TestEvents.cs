using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class TestEvent_InitFinish : AEvent<AppStartInitFinish>
    {
        protected override async ETTask Run(Scene scene, AppStartInitFinish a)
        {
            await ETTask.CompletedTask;
        }
    }
}
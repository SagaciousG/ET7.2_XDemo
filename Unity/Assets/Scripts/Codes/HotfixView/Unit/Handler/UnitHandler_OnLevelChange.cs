namespace ET.Client
{
    [Event(SceneType.Current)]
    public class UnitHandler_OnLevelChange : AEvent<UnitLevelChange>
    {
        protected override async ETTask Run(Scene scene, UnitLevelChange a)
        {
            UIHelper.PopTips(scene, "升级");
            await ETTask.CompletedTask;
        }
    }
}
namespace ET.Client
{
    [Event(SceneType.Battle)]
    public class SceneEvent_CreateBattle: AEvent<Event_AfterCreateBattleScene>
    {
        protected override async ETTask Run(Scene scene, Event_AfterCreateBattleScene args)
        {
            scene.AddComponent<UIComponent>();
            scene.AddComponent<BattleCameraComponent, int>(scene.GetParent<CurrentScenesComponent>().MapId);
            
            await ETTask.CompletedTask;
        }
    }
}
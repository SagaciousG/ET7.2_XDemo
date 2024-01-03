namespace ET.Client
{
    [Event(SceneType.Current)]
    public class SceneEvent_CreateCurrent: AEvent<Event_AfterCreateCurrentScene>
    {
        protected override async ETTask Run(Scene scene, Event_AfterCreateCurrentScene args)
        {
            scene.AddComponent<UIComponent>();
            scene.AddComponent<OperaComponent>();
            scene.AddComponent<UnitComponent>();
            scene.AddComponent<CameraComponent>();

            UIHelper.Create(UIType.UIHeaderContainer, scene, UILayer.Low).Coroutine();
            UIHelper.Create(UIType.UIMain, scene, UILayer.Mid).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}
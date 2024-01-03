using ET;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AfterCreateClientScene_AddComponent: AEvent<Event_AfterCreateClientScene>
    {
        protected override async ETTask Run(Scene scene, Event_AfterCreateClientScene args)
        {
            scene.AddComponent<InputComponent>();
            scene.AddComponent<UIEventComponent>();
            scene.AddComponent<UIComponent>();
            scene.AddComponent<HttpClientComponent>();

            EffectComponent.Instance = scene.AddComponent<EffectComponent>();
            await ETTask.CompletedTask;
        }
    }
}
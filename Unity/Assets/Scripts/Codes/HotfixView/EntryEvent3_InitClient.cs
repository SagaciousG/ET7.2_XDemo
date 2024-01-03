using System;
using System.IO;
using ET;

namespace ET.Client
{
    [Event(SceneType.Process)]
    public class EntryEvent3_InitClient: AEvent<ET.EventType.InitClient>
    {
        protected override async ETTask Run(Scene scene, ET.EventType.InitClient args)
        {
            Root.Instance.Scene.AddComponent<GlobalComponent>();

            Scene clientScene = await SceneFactory.CreateClientScene(1, "Client");
            
            await EventSystem.Instance.PublishAsync(clientScene, new AppStartInitFinish());
        }
    }
}
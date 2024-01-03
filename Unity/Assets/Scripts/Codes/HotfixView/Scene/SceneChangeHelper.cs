using UnityEngine.SceneManagement;

namespace ET.Client
{
    public static class SceneChangeHelper
    {
        // 场景切换协程
        public static async ETTask SceneChangeTo(Scene clientScene, int mapId, long sceneInstanceId)
        {
            CurrentScenesComponent currentScenesComponent = clientScene.GetComponent<CurrentScenesComponent>();
            currentScenesComponent.Current?.Dispose(); // 删除之前的CurrentScene，创建新的
            Scene currentScene = SceneFactory.CreateCurrentScene(sceneInstanceId, clientScene.Zone, mapId, currentScenesComponent);

            var waiter = currentScene.AddWaiter(WaiterKey.CreateMyUnit);
            // 可以订阅这个事件中创建Loading界面
            await EventSystem.Instance.PublishAsync(currentScene, new StartChangeScene());
            // 加载场景资源
            var waitCreateMyUnit = clientScene.GetComponent<ObjectWait>().Wait<Wait_CreateMyUnit>();
            var unitySceneComponent = currentScene.AddComponent<UnitySceneComponent, int>(mapId);
            await unitySceneComponent.Load(LoadSceneMode.Single);
            await EventSystem.Instance.PublishAsync(currentScene, new ChangeSceneFinish());

            // 等待CreateMyUnit的消息
            var m2CCreateMyUnit = (await waitCreateMyUnit).Message;
            UnitFactory.Create(currentScene, m2CCreateMyUnit.Unit);
            
            waiter.Dispatch();
            

            await EventSystem.Instance.PublishAsync(currentScene, new Event_AfterCreateMyUnit());
        }
    }
}
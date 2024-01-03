using System.Net.Sockets;

namespace ET.Client
{
    public static class SceneFactory
    {
        public static async ETTask<Scene> CreateClientScene(int zone, string name)
        {
            await ETTask.CompletedTask;
            
            Scene clientScene = EntitySceneFactory.CreateScene(zone, SceneType.Client, name, ClientSceneManagerComponent.Instance);
            clientScene.AddComponent<CurrentScenesComponent>();
            clientScene.AddComponent<ObjectWait>();
            clientScene.AddComponent<PlayerComponent>();
            
            clientScene.AddComponent<SessionComponent>();

            clientScene.AddComponent<SignalHoleComponent>();
            
            EventSystem.Instance.Publish(clientScene, new Event_AfterCreateClientScene());
            return clientScene;
        }
        
        public static Scene CreateCurrentScene(long id, int zone, int mapId, CurrentScenesComponent currentScenesComponent)
        {
            var cfg = MapConfigCategory.Instance.Get(mapId);
            Scene currentScene = EntitySceneFactory.CreateScene(id, IdGenerater.Instance.GenerateInstanceId(), zone, SceneType.Current, cfg.SceneName, currentScenesComponent);
            currentScenesComponent.Current = currentScene;
            currentScenesComponent.MapId = mapId;
            
            currentScene.AddComponent<SignalHoleComponent>();
            
            EventSystem.Instance.Publish(currentScene, new Event_AfterCreateCurrentScene());
            return currentScene;
        }
        
        public static Scene CreateBattleScene(long id, int zone, int mapId, CurrentScenesComponent currentScenesComponent)
        {
            var cfg = MapConfigCategory.Instance.Get(mapId);
            Scene battleScene = EntitySceneFactory.CreateScene(id, IdGenerater.Instance.GenerateInstanceId(), zone, SceneType.Battle, $"{cfg.SceneName}_battle", currentScenesComponent);
            currentScenesComponent.BattleScene = battleScene;
            
            
            battleScene.AddComponent<SignalHoleComponent>();

            EventSystem.Instance.Publish(battleScene, new Event_AfterCreateBattleScene());
            return battleScene;
        }
        
        public static Scene CreateBuildScene(CurrentScenesComponent currentScenesComponent)
        {
            var cfg = MapConfigCategory.Instance.Get(999);
            Scene buildScene = EntitySceneFactory.CreateScene(999, IdGenerater.Instance.GenerateInstanceId(), currentScenesComponent.DomainZone(), SceneType.Build, $"{cfg.SceneName}_build", currentScenesComponent);
            currentScenesComponent.BuildScene = buildScene;
            
            buildScene.AddComponent<SignalHoleComponent>();

   
            return buildScene;
        }
        
        public static Scene CreateGameScene(CurrentScenesComponent currentScenesComponent)
        {
            var cfg = MapConfigCategory.Instance.Get(999);
            Scene scene = EntitySceneFactory.CreateScene(999, IdGenerater.Instance.GenerateInstanceId(), currentScenesComponent.DomainZone(), SceneType.Build, $"{cfg.SceneName}_build", currentScenesComponent);
            currentScenesComponent.GameScene = scene;
            
            scene.AddComponent<SignalHoleComponent>();

   
            return scene;
        }
    }
}
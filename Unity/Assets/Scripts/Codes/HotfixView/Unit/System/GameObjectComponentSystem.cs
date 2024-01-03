using UnityEngine;

namespace ET.Client
{
    public static class GameObjectComponentSystem
    {
        public class GameObjectComponentAwakeSystem : AwakeSystem<GameObjectComponent, string>
        {
            protected override void Awake(GameObjectComponent self, string a)
            {
                self.Agent = self.AddChild<GameObjectAgent, string>(a);
            }
        }
        
        public static GameObject CreateEmpty(this GameObjectComponent self, string key, bool dontDestroy)
        {
            return self.Agent.CreateEmpty(key, dontDestroy);
        }

        public static ETTask<GameObject> Load(this GameObjectComponent self, string nameOrPath,
            Transform parent = null)
        {
            return self.Agent.Load(nameOrPath, parent);
        }
    }
}
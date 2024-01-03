using UnityEngine;
using UnityEngine.SceneManagement;
using ET;

namespace ET.Client
{
    [FriendOf(typeof(GameObjectAgent))]
    public static class GameObjectAgentSystem
    {
        public class GameObjectAgentAwakeSystem : AwakeSystem<GameObjectAgent, string>
        {
            protected override void Awake(GameObjectAgent self, string a)
            {
                self.ObjName = a;
            }
        }
        
        [ObjectSystem]
        public class GameObjectAgentDestroySystem: DestroySystem<GameObjectAgent>
        {
            protected override void Destroy(GameObjectAgent self)
            {
                if (self.Loaded && self.gameObject != null)
                {
                    GameObjectPool.Instance.Collect(self.AssetPath, self.gameObject);
                }

                self.gameObject = null;
                self.ParentTrans = null;
                self.Loaded = false;
                self.AssetPath = null;
                self.ObjName = null;
                self.IsEmptyObj = false;
            }
        }

        public static GameObject CreateEmpty(this GameObjectAgent self, string key, bool dontDestroy)
        {
            self.IsEmptyObj = true;
            self.Loaded = true;
            self.AssetPath = $"[EMPTY]{key}";
            if (!GameObjectPool.Instance.Fetch(self.AssetPath, out var gameObject))
            {
                gameObject = new GameObject(self.ObjName);
            }

            self.gameObject = gameObject;
            AfterCreate(self, self.gameObject);
            if (dontDestroy)
                UnityEngine.Object.DontDestroyOnLoad(self.gameObject);
            return self.gameObject;
        }
        public static async ETTask<GameObject> Load(this GameObjectAgent self, string nameOrPath, Transform parent = null)
        {
            self.Loaded = false;
            self.IsEmptyObj = false;
            self.ParentTrans = parent;
            if (self.gameObject != null)
            {
                GameObjectPool.Instance.Collect(self.AssetPath, self.gameObject);
                self.gameObject = null;
            }
            self.AssetPath = $"{nameOrPath}";
            
            if (GameObjectPool.Instance.Fetch(nameOrPath, out var obj))
            {
                self.gameObject = obj;
                AfterCreate(self, self.gameObject);
                self.Loaded = true;
                return obj;
            }

            self.gameObject = await YooAssetHelper.LoadGameObjectAsync(nameOrPath, Package.Art);
            if (self.IsDisposed)
            {
                GameObjectPool.Instance.Collect(nameOrPath, self.gameObject);
                self.gameObject = null;
                return null;
            }

            if (self.gameObject == null)
                return null;
            self.Loaded = true;
            AfterCreate(self, self.gameObject);
            return self.gameObject;
        }

        private static void AfterCreate(GameObjectAgent self, GameObject obj)
        {
            if (self.ParentTrans == null)
            {
                obj.transform.SetParent(null, false);
                var sceneComponent = self.DomainScene().GetComponent<UnitySceneComponent>();
                if (sceneComponent != null)
                {
                    SceneManager.MoveGameObjectToScene(obj, sceneComponent.Scene);
                }
            }
            else
            {
                obj.transform.SetParent(self.ParentTrans, false);
            }
        }
    }
}
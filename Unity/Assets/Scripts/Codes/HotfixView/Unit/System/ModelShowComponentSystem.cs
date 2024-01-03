using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET.Client
{
    public static class ModelShowComponentSystem 
    {
        public class ModelShowComponentAwakeSystem : AwakeSystem<ModelShowComponent, int>
        {
            protected override void Awake(ModelShowComponent self, int a)
            {
                self.Config = UnitShowConfigCategory.Instance.Get(a);
                self.AddComponent<GameObjectComponent, string>(self.Config.Model);
                self.ShellWaiter = self.AddWaiter(WaiterKey.ShellWaiter);
                self.ViewGOWaiter = self.AddWaiter(WaiterKey.ViewGOWaiter);
            }
        }
        
        public class DestroySystem: DestroySystem<ModelShowComponent>
        {
            protected override void Destroy(ModelShowComponent self)
            {
                if (self.Loaded)
                {
                    GameObjectPool.Instance.Collect(self.Config.Shell, self.Shell);
                }

                self.Shell = null;
                self.ViewGO = null;
                self.Loaded = false;
            }
        }
        
        public static async ETTask<GameObject> Load(this ModelShowComponent self, Transform parent = null)
        {
            self.Loaded = false;
            if (self.Shell != null)
            {
                GameObjectPool.Instance.Collect(self.Config.Shell, self.Shell);
                self.Shell = null;
            }
            
            if (GameObjectPool.Instance.Fetch(self.Config.Shell, out var obj))
            {
                self.Shell = obj;
                AfterCreate(self, obj, parent);
                self.Loaded = true;
                self.ShellWaiter.Dispatch();
                self.ShellWaiter = null;
                return obj;
            }

            obj = await YooAssetHelper.LoadGameObjectAsync(self.Config.Shell, Package.Art);
            self.Shell = obj;
            if (self.IsDisposed)
            {
                GameObjectPool.Instance.Collect(self.Config.Shell, self.Shell);
                self.Shell = null;
                self.ShellWaiter.Dispatch();
                self.ShellWaiter = null;
                return null;
            }

            if (self.Shell == null)
            {
                self.ShellWaiter.Dispatch();
                self.ShellWaiter = null;
                return null;
            }
            self.Loaded = true;
            AfterCreate(self, obj, parent);
            self.Shell.name = self.Config.Shell;
            self.ShellWaiter.Dispatch();
            self.ShellWaiter = null;
            return self.Shell;
        }

        private static void AfterCreate(ModelShowComponent self, GameObject obj, Transform parent)
        {
            var sceneComponent = self.DomainScene().GetComponent<UnitySceneComponent>();
            if (parent == null)
            {
                obj.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(obj, sceneComponent.Scene);
            }
            else
                obj.transform.SetParent(parent, false);

            obj.transform.localScale = self.Config.Scale * Vector3.one;
            obj.transform.position = self.GetParent<Unit>().Position;
            obj.transform.rotation = self.GetParent<Unit>().Rotation;
            self.Outline = self.Shell.AddComponent<QuickOutline>();
            self.Outline.enabled = false;
            
        }
        
        public static async ETTask<GameObject> LoadModel(this ModelShowComponent self)
        {
            self.ViewGO = await self.GetComponent<GameObjectComponent>().Load(self.Config.Model, self.Shell.transform);
            self.ViewGOWaiter.Dispatch();
            self.ViewGOWaiter = null;
            self.Outline.FindRenderer();
            return self.ViewGO;
        }
    }
}
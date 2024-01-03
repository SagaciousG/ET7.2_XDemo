using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET.Client
{
    [FriendOf(typeof(EffectItem))]
    public static class EffectItemSystem
    {
        public static async ETTask<GameObject> Load(this EffectItem self)
        {
            if (self.Loaded)
                return self.EffGO;
            if (GameObjectPool.Instance.Fetch(self.AssetName, out var obj))
            {
                self.Loaded = true;
                self.EffGO = obj;
            }
            else
            {
                self.EffGO = await YooAssetHelper.LoadGameObjectAsync(self.AssetName, Package.Art);
                self.Loaded = true;
                if (self.IsDisposed)
                {
                    GameObjectPool.Instance.Collect(self.AssetName, self.EffGO);
                    self.EffGO = null;
                    return null;
                }

                if (self.EffGO == null)
                    return null;
            }
            
            if (self.ParentTrans == null)
            {
                obj.transform.SetParent(null);
                var sceneComponent = self.DomainScene().GetComponent<UnitySceneComponent>();
                SceneManager.MoveGameObjectToScene(obj, sceneComponent.Scene);
            }
            else
                obj.transform.SetParent(self.ParentTrans, false);

            self.EffGO.transform.localScale = self.Scale;
            self.EffGO.transform.localPosition = self.Offset;
            self.EffGO.transform.rotation = Quaternion.Euler(self.Rotation);
            
            AutoDispose(self);
            return self.EffGO;
        }

        private static async void AutoDispose(EffectItem self)
        {
            if (self.LiveTime == 0)
                return;
            self.DisposeToken = new ETCancellationToken();
            await TimerComponent.Instance.WaitAsync(self.LiveTime, self.DisposeToken);
            if (self.DisposeToken.IsCancel())
                return;
            self.Dispose();
        }
        
        public class EffectItemAwakeSystem : AwakeSystem<EffectItem, string, Transform>
        {
            protected override void Awake(EffectItem self, string a, Transform t)
            {
                self.AssetName = a;
                self.ParentTrans = t;
                self.Loaded = false;
            }
        }
        
        public class EffectItemDestroySystem : DestroySystem<EffectItem>
        {
            protected override void Destroy(EffectItem self)
            {
                if (self.EffGO != null)
                {
                    GameObjectPool.Instance.Collect(self.AssetName, self.EffGO);
                    self.EffGO = null;
                    self.Loaded = false;
                }
            }
        }
    }
}
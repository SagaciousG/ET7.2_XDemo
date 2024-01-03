using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET.Client
{
    [FriendOf(typeof(UnitySceneComponent))]
    public static class UnitySceneComponentSystem
    {
        public class UnitySceneComponentAwakeSystem : AwakeSystem<UnitySceneComponent, int>
        {
            protected override void Awake(UnitySceneComponent self, int mapId)
            {
                var cfg = MapConfigCategory.Instance.Get(mapId);
                self.MapID = mapId;
                self.SceneName = cfg.SceneName;
            }
        }

        public class UnitySceneComponentDestroySystem : DestroySystem<UnitySceneComponent>
        {
            protected override void Destroy(UnitySceneComponent self)
            {
                SceneManager.UnloadSceneAsync(self.SceneName);
            }
        }
        
        public static async ETTask Load(this UnitySceneComponent self, LoadSceneMode mode)
        {
            var operationHandle = await YooAssetHelper.LoadSceneAsync($"{self.SceneName}", Package.Art, mode);
            operationHandle.UnSuspend();
            operationHandle.ActivateScene();
            self.Scene = SceneManager.GetSceneByName(self.SceneName);
        }

        public static void SetSceneActive(this UnitySceneComponent self, bool active)
        {
            var scene = SceneManager.GetSceneByName(self.SceneName);
            if (active)
            {
                foreach (GameObject obj in scene.GetRootGameObjects())
                {
                    if (self.ActivedRootObjs.Contains(obj))
                        obj.SetActive(true);
                }
            }
            else
            {
                self.ActivedRootObjs.Clear();
                foreach (GameObject obj in scene.GetRootGameObjects())
                {
                    if (obj.activeSelf)
                    {
                        self.ActivedRootObjs.Add(obj);
                    }
                    obj.SetActive(false);
                }
            }
        }
    }
}
using UnityEngine;
using ET;

namespace ET.Client
{
    public static class BattleCameraComponentSystem
    {
        public class AwakeSystem : AwakeSystem<BattleCameraComponent, int>
        {
            protected override async void Awake(BattleCameraComponent self, int a)
            {
                self.MainCamera = Init.Instance.MainCamera;
                var map = MapConfigCategory.Instance.Get(a);
                self.MainCamera.transform.position = map.BattleCameraPos.ToVector3();
                self.MainCamera.transform.rotation = Quaternion.Euler(map.BattleCameraRot.ToVector3());
                
            }
        }
        
        
    }
}
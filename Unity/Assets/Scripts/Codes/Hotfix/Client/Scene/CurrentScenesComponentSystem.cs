using System;

namespace ET.Client
{
    [FriendOf(typeof(CurrentScenesComponent))]
    public static class CurrentScenesComponentSystem
    {
        public class CurrentScenesComponentAwakeSystem : AwakeSystem<CurrentScenesComponent>
        {
            protected override void Awake(CurrentScenesComponent self)
            {
            }
        }
        
        public static Scene CurrentScene(this Scene clientScene)
        {
            return clientScene.GetComponent<CurrentScenesComponent>()?.Current;
        }
        
        
    }
}
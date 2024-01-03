namespace ET.Server
{

    public static class RealmPlayerSystem
    {
        public class RealmPlayerAwakeSystem : AwakeSystem<RealmPlayer>
        {
            protected override void Awake(RealmPlayer self)
            {
                
            }
        }
        
        public class RealmPlayerDestroySystem : DestroySystem<RealmPlayer>
        {
            protected override void Destroy(RealmPlayer self)
            {
                Log.Console($"[Realm] player Disconnect {self.Account}");
                var onlinePlayerComponent = self.DomainScene().GetComponent<OnlinePlayerComponent>();
                onlinePlayerComponent.Remove(self.Account);
            }
        }
        
        
    }

}
namespace ET.Server
{
    public class OnlinePlayerInfoAwakeSystem: AwakeSystem<OnlinePlayerInfo, string>
    {
        protected override void Awake(OnlinePlayerInfo self, string account)
        {
            self.Account = account;
        }
    }
    [FriendOf(typeof(OnlinePlayerInfo))]
    public static class OnlinePlayerInfoSystem
    {
    
    }
}
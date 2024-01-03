namespace ET.Server
{
    public class OnlinePlayerComponentAwakeSystem : AwakeSystem<OnlinePlayerComponent>
    {
        protected override void Awake(OnlinePlayerComponent self)
        {
            
        }
    }

    [FriendOf(typeof(OnlinePlayerComponent))]
    public static class OnlinePlayerComponentSystem
    {
        public static OnlinePlayerInfo Add(this OnlinePlayerComponent self, string account)
        {
            var playerInfo = self.AddChild<OnlinePlayerInfo, string>(account);
            self.Players.Add(account, playerInfo);
            return playerInfo;
        }

        public static void Remove(this OnlinePlayerComponent self, string account)
        {
            if (self.Players.TryGetValue(account, out var playerInfo))
            {
                self.RemoveChild(playerInfo.Id);
                self.Players.Remove(account);
            }
        }

        public static OnlinePlayerInfo Get(this OnlinePlayerComponent self, string account)
        {
            self.Players.TryGetValue(account, out var player);
            return player;
        }

        public static bool Contain(this OnlinePlayerComponent self, string account)
        {
            return self.Players.ContainsKey(account);
        }
    }
}
namespace ET.Server
{
    public class ZoneComponentAwakeSystem: AwakeSystem<ZoneComponent>
    {
        protected override void Awake(ZoneComponent self)
        {
    
        }
    }



    [FriendOf(typeof(ZoneComponent))]
    [FriendOf(typeof(ZoneInfo))]
    public static class ZoneComponentSystem
    {
        public static ZoneInfo Get(this ZoneComponent self, int zone)
        {
            self.Zones.TryGetValue(zone, out var info);
            return info;
        }
        public static void ChangeState(this ZoneComponent self, int zone, int online)
        {
            if (!self.Zones.TryGetValue(zone, out var zoneInfo))
            {
                var cfg = StartSceneConfigCategory.Instance.GetGate(zone);
                if (cfg != null)
                {
                    zoneInfo = self.AddChild<ZoneInfo, int, long>(zone, cfg.InstanceId);
                    self.Zones.Add(zone, zoneInfo);
                }
            }
            zoneInfo.IsOnline = online == 1;
            if (online == 0)
            {
                zoneInfo.PlayerIds.Clear();
            }
        }
        
        public static void AddToZone(this ZoneComponent self, int zone, long playerId)
        {
            if (self.Zones.TryGetValue(zone, out var zoneInfo))
            {
                zoneInfo.Add(playerId);
            }
        }
        
        public static void RemoveFromZone(this ZoneComponent self, int zone, long playerId)
        {
            if (self.Zones.TryGetValue(zone, out var zoneInfo))
            {
                zoneInfo.Remove(playerId);
            }
        }
    }
}
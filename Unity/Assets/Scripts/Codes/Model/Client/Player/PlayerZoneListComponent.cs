using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(PlayerComponent))]
    public class PlayerZoneListComponent : Entity, IAwake
    {
        public List<ZoneInfo> ZoneInfos { get; set; }
        public List<int> LatestEnterZone { get; set; }
    }
}
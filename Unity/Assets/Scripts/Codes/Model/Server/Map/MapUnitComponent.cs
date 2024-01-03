using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Map))]
    public class MapUnitComponent : Entity, IAwake
    {
        public Dictionary<long, Unit> MapUnits { get; set; } = new();
    }
}
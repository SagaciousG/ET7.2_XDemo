using System;

namespace ET
{
    public partial class MapBuildConfigCategory
    {
        private MultiMap<int, MapBuildConfig> _groupCfg = new();

        public override void AfterEndInit()
        {
            foreach (var config in list)
            {
                _groupCfg.Add(config.Type, config);
            }
        }

        public MapBuildConfig[] GetByType(int type)
        {
            _groupCfg.TryGetValue(type, out var res);
            return res?.ToArray() ?? Array.Empty<MapBuildConfig>();
        }
    }
}
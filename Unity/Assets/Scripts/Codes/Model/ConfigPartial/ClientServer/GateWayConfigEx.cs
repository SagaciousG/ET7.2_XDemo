using System.Collections.Generic;

namespace ET
{
    public partial class GateWayConfigCategory
    {
        private MultiMap<int, GateWayConfig> _groupCfgs = new MultiMap<int, GateWayConfig>();

        public List<GateWayConfig> GetByMap(int map)
        {
            this._groupCfgs.TryGetValue(map, out var res);
            return res ?? new List<GateWayConfig>();
        }

        public override void AfterEndInit()
        {
            foreach (var kv in this.dict)
            {
                this._groupCfgs.Add(kv.Value.Map, kv.Value);
            }
        }
    }
    
    public partial class GateWayConfig
    {
    
    }
}
using System;

namespace ET
{
    
    public partial class UnitShowConfigCategory
    {
        private MultiMap<int, UnitShowConfig> _groupCfgs = new MultiMap<int, UnitShowConfig>();

        public UnitShowConfig[] GetByGroup(int group)
        {
            if (this._groupCfgs.TryGetValue(group, out var res))
            {
                return res.ToArray();
            }

            return Array.Empty<UnitShowConfig>();
        }

        public override void AfterEndInit()
        {
            foreach (UnitShowConfig cfg in this.dict.Values)
            {
                this._groupCfgs.Add(cfg.Group, cfg);
            }
        }
    }
}
using System.Collections.Generic;

namespace ET
{
    public partial class SkillBaseConfigCategory
    {
        private MultiMap<int, SkillBaseConfig> _groupCfgs = new MultiMap<int, SkillBaseConfig>();
        private MultiDictionary<int, int, SkillBaseConfig> _groupLv = new();
        public List<SkillBaseConfig> GetByGroup(int group)
        {
            this._groupCfgs.TryGetValue(group, out var res);
            return res ?? new List<SkillBaseConfig>();
        }

        public SkillBaseConfig GetByGroupLv(int group, int lv)
        {
            _groupLv.TryGetValue(group, lv, out var res);
            return res;
        }

        public override void AfterEndInit()
        {
            foreach (var kv in this.dict)
            {
                this._groupCfgs.Add(kv.Value.Group, kv.Value);
                _groupLv.Add(kv.Value.Group, kv.Value.Level, kv.Value);
            }
        }
    }
}
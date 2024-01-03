using System;

namespace ET
{
    public partial class MonsterGroupConfigCategory
    {
        private MultiMap<int, MonsterGroupConfig> _groupMonster = new MultiMap<int, MonsterGroupConfig>();
        public override void AfterEndInit()
        {
            foreach (var kv in this.dict)
            {
                this._groupMonster.Add(kv.Value.Group, kv.Value);
            }
        }

        public MonsterGroupConfig[] GetByGroup(int group)
        {
            this._groupMonster.TryGetValue(group, out var res);
            return res?.ToArray() ?? Array.Empty<MonsterGroupConfig>();
        }
    }
}
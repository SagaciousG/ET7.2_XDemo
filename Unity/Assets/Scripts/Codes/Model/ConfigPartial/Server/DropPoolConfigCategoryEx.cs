using System.Collections.Generic;
using ET.Server;

namespace ET
{
    public partial class DropPoolConfigCategory
    {
        private MultiMap<int, DropPoolConfig> _groupConfigs = new();
        private Dictionary<int, int> _groupWeight = new();
        public override void AfterEndInit()
        {
            foreach (var kv in dict)
            {
                var dropPoolConfig = kv.Value;
                _groupConfigs.Add(dropPoolConfig.Group, dropPoolConfig);
                _groupWeight.TryGetValue(dropPoolConfig.Group, out var weight);
                _groupWeight[dropPoolConfig.Group] = weight + dropPoolConfig.Weight;
            }
        }

        public DropPoolConfig[] GetByGroup(int group)
        {
            return _groupConfigs[group].ToArray();
        }

        public int GetWeight(int group)
        {
            return _groupWeight[group];
        }
    }
}
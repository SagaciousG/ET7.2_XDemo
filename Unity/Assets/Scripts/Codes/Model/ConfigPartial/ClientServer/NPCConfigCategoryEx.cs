namespace ET
{
    public partial class NPCConfigCategory
    {
        private MultiMap<int, NPCConfig> _mapNPCs = new();

        public override void AfterEndInit()
        {
            foreach (var kv in dict)
            {
                _mapNPCs.Add(kv.Value.Map, kv.Value);
            }
        }

        public NPCConfig[] GetNPCByMap(int map)
        {
            return _mapNPCs[map].ToArray();
        }
    }
}
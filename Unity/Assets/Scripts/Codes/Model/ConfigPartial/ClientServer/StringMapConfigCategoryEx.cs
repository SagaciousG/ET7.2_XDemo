namespace ET
{
    public partial class StringMapConfigCategory
    {
        private MultiDictionary<StringMapType, int, string> map = new();
        public override void AfterEndInit()
        {
            foreach (var config in list)
            {
                map.Add((StringMapType)config.Group, config.Key, config.Val);
            }
        }

        public string GetVal(StringMapType type, int key)
        {
            map.TryGetValue(type, key, out var res);
            return res;
        }
    }

    public enum StringMapType
    {
        EquipHole,
        ItemQuality,
    }
}
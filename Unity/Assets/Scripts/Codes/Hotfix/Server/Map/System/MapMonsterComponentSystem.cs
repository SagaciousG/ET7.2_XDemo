namespace ET.Server
{
    [FriendOf(typeof(MapMonsterComponent))]
    public static class MapMonsterComponentSystem 
    {
        public class MapMonsterComponentAwakeSystem : AwakeSystem<MapMonsterComponent, int>
        {
            protected override void Awake(MapMonsterComponent self, int a)
            {
                self.MonsterGroup = a;
                var groups = MonsterGroupConfigCategory.Instance.GetByGroup(self.MonsterGroup);
                self.WeightTotal = 0;
                foreach (MonsterGroupConfig cfg in groups)
                {
                    self.WeightTotal += cfg.Weight;
                }
            }
        }

        public static MonsterGroupConfig RandomGet(this MapMonsterComponent self)
        {
            var num = RandomGenerator.RandomNumber(0, self.WeightTotal);
            var groups = MonsterGroupConfigCategory.Instance.GetByGroup(self.MonsterGroup);
            foreach (MonsterGroupConfig config in groups)
            {
                num -= config.Weight;
                if (num <= 0)
                    return config;
            }

            return null;
        }
    
    }
}
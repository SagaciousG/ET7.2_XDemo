using System;

namespace ET.Server
{
    public static class LotteryComponentSystem
    {
        public static int GetGroup(this LotteryComponent self, LotteryType type)
        {
            return (int)type;
        }

        public static int DoLottery(this LotteryComponent self, LotteryType type, int group)
        {
            var random = self.Randoms[type];
            var drops = DropPoolConfigCategory.Instance.GetByGroup(group);
            var res = random.Next(0, DropPoolConfigCategory.Instance.GetWeight(group));
            for (int i = 0; i < drops.Length; i++)
            {
                var d = drops[i];
                res -= d.Weight;
                if (res <= 0)
                    return d.Id;
            }

            return -1;
        }
        
        
        public class LotteryComponentDeserialize : DeserializeSystem<LotteryComponent>
        {
            protected override void Deserialize(LotteryComponent self)
            {
                foreach (var kv in self.RandomSeeds)
                {
                    self.Randoms[kv.Key] = new Random(kv.Value);
                    SeekToCurrent(self.Randoms[kv.Key], self.LotteryCounts[kv.Key]).Coroutine();
                }
            }
            
            private ETTask SeekToCurrent(Random random, long count)
            {
                var task = ETTask.Create();
                for (int i = 0; i < count; i++)
                {
                    random.Next();
                }

                return task;
            }
        }
    }
}
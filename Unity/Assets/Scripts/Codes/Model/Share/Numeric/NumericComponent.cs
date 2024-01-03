using System.Collections.Generic;
using System.Linq;
using ET.EventType;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [FriendOf(typeof (NumericComponent))]
    public static class NumericComponentSystem
    {
        public static bool HasVal(this NumericComponent self, int numericType)
        {
            return self.numericDic.ContainsKey(numericType);
        }
        public static float GetAsFloat(this NumericComponent self, int numericType)
        {
            return (float)self.GetByKey(numericType) / 10000;
        }

        public static int GetAsInt(this NumericComponent self, int numericType)
        {
            return (int)self.GetByKey(numericType);
        }
        
        public static long GetAsLong(this NumericComponent self, int numericType)
        {
            return self.GetByKey(numericType);
        }

        public static void Set(this NumericComponent self, int nt, float value, int flag = 0)
        {
            self.Insert(nt, (int)(value * 10000), true, flag);
        }

        public static void Set(this NumericComponent self, int nt, int value, int flag = 0)
        {
            self.Insert(nt, value, true, flag);
        }

        public static void Set(this NumericComponent self, int nt, long value, int flag = 0)
        {
            self.Insert(nt, value, true, flag);
        }

        public static void SetNoEvent(this NumericComponent self, int numericType, long value)
        {
            self.Insert(numericType, value, false);
        }

        private static void Insert(this NumericComponent self, int numericType, long value, bool isPulishEvent = true, int flag = 0)
        {
            long oldValue = self.GetByKey(numericType);
            if (numericType <= NumericType.Max)
            {
                Log.Error($"不允许设置最终变量的值：{numericType}");
                return;
            }
            self.numericDic[numericType] = value;
            
            int final = (int)numericType / 10;
            int bas = final * 10 + 1;
            int add = final * 10 + 2;
            int pct = final * 10 + 3;
            int finalAdd = final * 10 + 4;
            int finalPct = final * 10 + 5;

            // 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
            // final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;
            long result = (long)(((self.GetByKey(bas) + self.GetByKey(add)) * (100 + self.GetAsFloat(pct)) / 100f + self.GetByKey(finalAdd)) *
                (100 + self.GetAsFloat(finalPct)) / 100f);

            var oldFinal = self.GetByKey(final);
            self.numericDic[final] = result;

            if (isPulishEvent)
            {
                NumericWatcherComponent.Instance.Run(self, new NumbericChange(){New = value, Old = oldValue, NumericType = numericType, Flag = flag});
                NumericWatcherComponent.Instance.Run(self, new NumbericChange(){New = result, Old = oldFinal, NumericType = final, Flag = flag});
            }
        }

        public static long GetByKey(this NumericComponent self, int key)
        {
            self.numericDic.TryGetValue(key, out var value);
            return value;
        }
    }
    
    namespace EventType
    {
        public struct NumbericChange
        {
            public int NumericType;
            public long Old;
            public long New;
            public int Flag;
        }
    }

    [ComponentOf()]
    public class NumericComponent: Entity, IAwake, ITransfer
    {
        public Dictionary<int, long> NumericDic => numericDic;
        
        [BsonIgnore]
        public Dictionary<int, long> numericDic = new();
    }
}
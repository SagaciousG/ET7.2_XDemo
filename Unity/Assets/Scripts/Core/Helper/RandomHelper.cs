using System;
using System.Collections.Generic;

namespace ET
{
    public static class RandomHelper
    {
        /// <summary>
        /// 获取lower与Upper之间的随机数,包含下限，不包含上限
        /// </summary>
        public static int RandomNumber(this Random random, int lower, int upper)
        {
            int value = random.Next(lower, upper);
            return value;
        }
        
        public static T RandomArray<T>(this Random random, T[] array)
        {
            return array[RandomNumber(random, 0, array.Length)];
        }

        public static T RandomArray<T>(this Random random, List<T> array)
        {
            return array[RandomNumber(random, 0, array.Count)];
        }

        /// <summary>
        /// 打乱数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="random"></param>
        /// <param name="arr">要打乱的数组</param>
        public static void BreakRank<T>(this Random random, IList<T> arr)
        {
            if (arr == null || arr.Count < 2)
            {
                return;
            }

            for (int i = 0; i < arr.Count; i++)
            {
                int index = random.Next(0, arr.Count);
                (arr[index], arr[i]) = (arr[i], arr[index]);
            }
        }
    }
}
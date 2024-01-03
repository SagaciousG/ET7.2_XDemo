using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    public static class BagComponentSystem
    {
        public static long Update(this BagComponent self, BagItemProto item, bool sendEvent = true)
        {
            var change = 0L;
            var child = self.GetChild<BagItem>(item.UID);
            if (child == null)
            {
                child = self.Add(item, sendEvent);
                change = item.Num;
            }
            else
            {
                change = item.Num - child.Num;
                child.Num = item.Num;
            }
            if (child.Num <= 0)
            {
                self.BagItems.Remove(item.ID, child);
                child.Dispose();
            }

            return change;
        }
    }
}
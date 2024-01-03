using System.Collections.Generic;

namespace ET.Client
{
    public static class ArmsComponentSystem
    {
        public static ArmsItem EquipUp(this ArmsComponent self, long uid, EquipmentHole hole)
        {
            if (self.HoleItem.TryGetValue(hole, out  var armsItem))
            {
                armsItem.Dispose();
            }

            var res = self.AddChildWithId<ArmsItem, EquipmentHole>(uid, hole);
            self.HoleItem[hole] = res;
            return res;
        }
        
    }
}
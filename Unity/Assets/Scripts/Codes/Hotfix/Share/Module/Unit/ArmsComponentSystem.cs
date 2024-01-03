using System.Collections.Generic;

namespace ET
{
    public static class ArmsComponentSystem
    {
        
        public static List<EquipmentProto> GetEquipmentProto(this ArmsComponent self)
        {
            var all = self.GetChildren<ArmsItem>();
            var list = new List<EquipmentProto>();
            foreach (var item in all)
            {
                list.Add(new EquipmentProto()
                {
                    UID = item.Id,
                });
            }

            return list;
        }
    }
}
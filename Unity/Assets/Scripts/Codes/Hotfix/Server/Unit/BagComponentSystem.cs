using System.Collections.Generic;

namespace ET.Server
{
    public static class BagComponentSystem
    {
        public static void SendBagItemChange(this BagComponent self, List<BagItemProto> items)
        {
            MessageHelper.SendToClient(self.GetParentDepth<Unit>(), new UnitBagAMessage(){Items = items});
            var arms = new List<BagItemArmsProto>();
            foreach (var itemProto in items)
            {
                var bagItem = self.GetByUID(itemProto.UID);
                var itemCfg = ItemConfigCategory.Instance.Get(itemProto.ID);
                switch ((BagItemType) itemCfg.Type)
                {
                    case BagItemType.Arms:
                        var bagItemArmComponent = bagItem.GetComponent<BagItemArmComponent>();
                        arms.Add(new BagItemArmsProto(){UID = itemProto.UID, Equipped = bagItemArmComponent.Equipped ? 1 : 0});
                        break;
                }
            }

            if (arms.Count > 0)
            {
                MessageHelper.SendToClient(self.GetParentDepth<Unit>(), new UnitBagArmsAMessage(){Arms = arms});
            }
        }
    }
}
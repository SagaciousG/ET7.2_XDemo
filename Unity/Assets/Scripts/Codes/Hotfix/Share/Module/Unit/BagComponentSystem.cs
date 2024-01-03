using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public static class BagComponentSystem
    {
        public class BagComponentDeserializeSystem : DeserializeSystem<BagComponent>
        {
            protected override void Deserialize(BagComponent self)
            {
                self.BagItems = new();
                foreach (var bagItem in self.GetChildren<BagItem>())
                {
                    self.BagItems.Add(bagItem.ItemID, bagItem);
                }
            }
        }

        public static BagItem GetByUID(this BagComponent self, long uid)
        {
            return self.GetChild<BagItem>(uid);
        }

        public static long GetNum(this BagComponent self, int id)
        {
            var count = 0L;
            if (self.BagItems.TryGetValue(id, out var bagItems))
            {
                foreach (var item in bagItems)
                {
                    count += item.Num;
                }
            }

            return count;
        }

        public static void Update(this BagComponent self, BagItemProto item, bool sendEvent = true)
        {
            var child = self.GetChild<BagItem>(item.UID);
            if (child == null)
            {
                self.Add(item, sendEvent);
            }
            else
            {
                child.Num = item.Num;
                if (child.Num <= 0)
                {
                    self.BagItems.Remove(item.ID, child);
                    child.Dispose();
                }
            }
        }

        public static BagItem Add(this BagComponent self, BagItemProto item, bool sendEvent = true)
        {
            var count = item.Num;
            var child = self.GetChild<BagItem>(item.UID);
            if (child == null)
            {
                child = self.AddChildWithId<BagItem, int>(item.UID, item.ID);
                self.BagItems.Add(item.ID, child);
            }

            child.Num += count;

            if (sendEvent)
            {
                EventSystem.Instance.Publish(self.DomainScene(), new Event_OnBagItemChange(){Items = new()
                {
                    new BagItemProto(){ID = item.ID, Num = child.Num, UID = item.UID}
                },
                    Unit = self.GetParent<Unit>()
                });
            }

            return child;
        }
        
        public static List<BagItemProto> Add(this BagComponent self, int id, int num, bool sendEvent = true)
        {
            var result = new List<BagItemProto>();
            long count = num;
            if (self.BagItems.TryGetValue(id, out var list))
            {
                foreach (var bagItem in list)
                {
                    if (bagItem.Num < bagItem.Config.Overlay || bagItem.Config.Overlay == 0)
                    {
                        var add = math.min(count, bagItem.Config.Overlay - bagItem.Num);
                        if (bagItem.Config.Overlay == 0)
                            add = count;
                        bagItem.Num += add;
                        count -= add;
                        result.Add(new BagItemProto()
                        {
                            ID = id,
                            Num = bagItem.Num,
                            UID = bagItem.Id
                        });
                    }
                    if (count <= 0)
                        break;
                }
            }

            if (count > 0)
            {
                var bagItem = self.AddChild<BagItem, int>(id);
                bagItem.Num = count;
                self.BagItems.Add(id, bagItem);
                result.Add(new BagItemProto()
                {
                    ID = id,
                    Num = bagItem.Num,
                    UID = bagItem.Id
                });
            }

            if (sendEvent)
            {
                EventSystem.Instance.Publish(self.DomainScene(), new Event_OnBagItemChange()
                {
                    Items = result,
                    Unit = self.GetParent<Unit>()
                });
            }
            return result;
        }

        public static void Remove(this BagComponent self, BagItemProto itemProto, bool sendEvent = true)
        {
            if (itemProto.UID > 0)
            {
                self.Remove(itemProto.UID, itemProto.Num, sendEvent);
            }
            else
            {
                self.Remove(itemProto.ID, itemProto.Num, sendEvent);
            }
        }

        public static void Remove(this BagComponent self, long uid, long num, bool sendEvent = true)
        {
            var item = self.GetChild<BagItem>(uid);
            if (item == null)
                return;
            item.Num -= num;
            var id = item.ItemID;
            var count = item.Num;
            if (item.Num <= 0)
            {
                self.BagItems.Remove(item.ItemID, item);
                item.Dispose();
            }

            if (sendEvent)
            {
                EventSystem.Instance.Publish(self.DomainScene(), new Event_OnBagItemChange(){Items = new()
                {
                    new BagItemProto()
                    {
                        ID = id,
                        UID = uid,
                        Num = count
                    }
                },
                    Unit = self.GetParent<Unit>()
                });
            }
        }
        
        public static void Remove(this BagComponent self, int id, long num, bool sendEvent = true)
        {
            long count = num;
            if (self.BagItems.TryGetValue(id, out var list))
            {
                var change = new List<BagItemProto>();
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    var bagItem = list[i];
                    var sub = math.min(count, bagItem.Num);
                    bagItem.Num -= sub;
                    count -= sub;

                    change.Add(new BagItemProto()
                    {
                        ID = id,
                        UID = bagItem.Id,
                        Num = bagItem.Num,
                    });
                    if (bagItem.Num == 0)
                    {
                        list.RemoveAt(i);
                        bagItem.Dispose();
                    }
                    if (count <= 0)
                        break;
                }

                if (list.Count == 0)
                    self.BagItems.Remove(id);
                if (sendEvent)
                {
                    EventSystem.Instance.Publish(self.DomainScene(), new Event_OnBagItemChange()
                    {
                        Items = change,
                        Unit = self.GetParent<Unit>()
                    });
                }
            }
        }
    }
}
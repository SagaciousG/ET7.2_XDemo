using System.Collections.Generic;

namespace ET.Server
{
    public static class ArmsComponentSystem
    {
        public static void EquipDown(this ArmsComponent self, long uid, bool sendEvent)
        {
            self.EquipDown(self.GetChild<ArmsItem>(uid), sendEvent);
        }
        public static void EquipDown(this ArmsComponent self, ArmsItem item, bool sendEvent)
        {
            var unit = self.GetParentDepth<Unit>();
            var bagItem = unit.GetComponent<BagComponent>().GetByUID(item.Id);
            
            var armCfg = ItemConfigCategory.Instance.Get(bagItem.ItemID).GetArm();
            var words = armCfg.BaseWord.ToIntArray(',');
            var wordVals = armCfg.BaseWordVal.ToIntArray(',');
            for (int i = 0; i < words.Length; i++)
            {
                EntryWordHelper.Parse(unit, words[i], wordVals[i], OperatorType.Sub);
            }

            self.HoleItem.Remove(item.Hole);
            if (sendEvent)
            {
                MessageHelper.SendToClient(unit, new UnitBagArmsAMessage()
                {
                    Arms = new()
                    {
                        new BagItemArmsProto()
                        {
                            UID = item.Id,
                            Equipped = 0,
                        }
                    }
                });
                MessageHelper.SendToClient(unit, new UnitEquipmentUpdateAMessage()
                {
                    Equips = new()
                    {
                        new EquipmentProto()
                        {
                            EquipUp = 0,
                            UID = item.Id,
                            ProfessionNum = (int)item.GetParentDepth<Profession>().Num,
                            Hole = (int)item.Hole,
                        }
                    }
                });
                MessageHelper.SendToClient(unit, new UnitNumericUpdateAMessage()
                {
                    Numeric = unit.GetComponent<NumericComponent>().NumericDic
                });
            }
            item.Dispose();
        }
        
        public static void EquipUp(this ArmsComponent self, BagItem item, int profession, EquipmentHole hole)
        {
            var unit = self.GetParentDepth<Unit>();
            var changeArms = new List<BagItemArmsProto>();
            if (self.HoleItem.TryGetValue(hole, out  var armsItem))
            {
                var bagItem = unit.GetComponent<BagComponent>().GetByUID(armsItem.Id);
                bagItem.GetComponent<BagItemArmComponent>().Equipped = false;
                self.EquipDown(armsItem, false);
                changeArms.Add(new BagItemArmsProto(){UID = armsItem.Id, Equipped = 0});
            }

            var equipUp = self.AddChildWithId<ArmsItem, EquipmentHole>(item.Id, hole);
            self.HoleItem[hole] = equipUp;
            item.GetComponent<BagItemArmComponent>().Equipped = true;
            changeArms.Add(new BagItemArmsProto(){UID = equipUp.Id, Equipped = 1});
            MessageHelper.SendToClient(unit, new UnitEquipmentUpdateAMessage()
            {
                Equips = new()
                {
                    new EquipmentProto()
                    {
                        EquipUp = 1,
                        UID = equipUp.Id,
                        ProfessionNum = profession,
                        Hole = (int)equipUp.Hole,
                    }
                }
            });
            MessageHelper.SendToClient(unit, new UnitBagArmsAMessage()
            {
                Arms = changeArms
            });
            var armCfg = ItemConfigCategory.Instance.Get(item.ItemID).GetArm();
            var words = armCfg.BaseWord.ToIntArray(',');
            var wordVals = armCfg.BaseWordVal.ToIntArray(',');
            for (int i = 0; i < words.Length; i++)
            {
                EntryWordHelper.Parse(unit, words[i], wordVals[i], OperatorType.Add);
            }
            MessageHelper.SendToClient(unit, new UnitNumericUpdateAMessage()
            {
                Numeric = unit.GetComponent<NumericComponent>().NumericDic
            });
        }

        public static void AfterDeserialize(this ArmsComponent self)
        {
            var unit = self.GetParentDepth<Unit>();
            var armsItems = self.GetChildren<ArmsItem>();
            var bag = unit.GetComponent<BagComponent>();
            foreach (var armsItem in armsItems)
            {
                var item = bag.GetByUID(armsItem.Id);
                var armCfg = ItemConfigCategory.Instance.Get(item.ItemID).GetArm();
                var words = armCfg.BaseWord.ToIntArray(',');
                var wordVals = armCfg.BaseWordVal.ToIntArray(',');
                for (int i = 0; i < words.Length; i++)
                {
                    EntryWordHelper.Parse(unit, words[i], wordVals[i], OperatorType.Add);
                }
                
                self.HoleItem.Add(armsItem.Hole, armsItem);
            }
        }
    }
}
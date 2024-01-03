
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ET;

namespace ET.Client
{
	
	[FriendOf(typeof(UIBagComponent))]
	public static class UIBagComponentSystem
	{
        public static void OnAwake(this UIBagComponent self)
        {
	        self.tabList.OnData += self.OnDataChanged;
	        self.tabList.OnSelectedViewRefresh += self.OnTabSelected;
        }
        
        private static void OnDataChanged(this UIBagComponent self, int index, RectTransform trans, object data)
        {
	        var rc = trans.GetComponent<UIReferenceCollector>();
	        var name = rc.Get<XText>("name");
	        Enum.TryParse<BagItemType>((string)data, out var e);
	        name.text = e.GetNameAttribute().Name;
        }

        private static void OnTabSelected(this UIBagComponent self, RectTransform trans, bool select, object data, int index)
        {
	        var rc = trans.GetComponent<UIReferenceCollector>();
	        var selected = rc.Get<XImage>("selected");
	        selected.Display(select);
	        if(!select)
		        return;
	        Enum.TryParse<BagItemType>((string)data, out var e);
	        self.TabOnType = (int)e;
	        self.RefreshShowList();
        }

        public static async void RefreshShowList(this UIBagComponent self)
        {
	        self.ShowList.Clear();
	        var myUnit = await UnitHelper.GetMyUnitFromClientScene(self.ClientScene());
	        var bagComponent = myUnit.GetComponent<BagComponent>();
	        if (self.TabOnType == (int)BagItemType.All)
	        {
		        foreach (var bagItem in bagComponent.GetChildren<BagItem>())
		        {
			        if (bagItem.Config.Type == (int)BagItemType.Arms)
			        {
				        if (bagItem.GetComponent<BagItemArmComponent>().Equipped)
							continue;
			        }
			        self.ShowList.Add(new UIBagItemArgs()
			        {
				        Id = bagItem.ItemID,
				        Num = bagItem.Num,
				        ShowState = UIBagItemShowState.InBag,
				        UID = bagItem.Id,
			        });
		        }
	        }
	        else
	        {
		        foreach (var bagItem in bagComponent.GetChildren<BagItem>())
		        {
			        if (bagItem.Config.Type == self.TabOnType)
			        {
				        if (bagItem.Config.Type == (int)BagItemType.Arms)
				        {
					        if (bagItem.GetComponent<BagItemArmComponent>().Equipped)
						        continue;
				        }
				        self.ShowList.Add(new UIBagItemArgs()
				        {
					        Id = bagItem.ItemID,
					        Num = bagItem.Num,
					        ShowState = UIBagItemShowState.InBag,
					        UID = bagItem.Id,
				        });
			        }
		        }
	        }
	        self.itemList.SetData(self.ShowList);
        }

        public static void OnCreate(this UIBagComponent self)
        {
	        UIHelper.CreateUIMoney(self.GetParent<UI>(), MoneyType.Copper).Coroutine();
	        
	        SignalHelper.Add(self.DomainScene(), SignalKey.UI_NumericChanged, self, self.OnNumericChange);
	        SignalHelper.Add<int>(self.DomainScene(), SignalKey.UI_EquipmentChanged_Hole, self, self.OnEquipmentChange);
        }

        private static void OnEquipmentChange(this UIBagComponent self, int hole)
        {
	        self.equipUI[hole].SetData(hole);
        }
        
        private static void OnNumericChange(this UIBagComponent self)
        {
	        self.attrList.UpdateViewCells();
        }
        
        public static void SetData(this UIBagComponent self, params object[] args)
        {
	        UIHelper.CreateUITopBack(self.GetParent<UI>(), "背包").Coroutine();
	        var enumNames = Enum.GetNames(typeof(BagItemType)).ToList();
	        enumNames.Remove("All");
	        enumNames.Insert(0, "All");
	        self.tabList.SetData(enumNames);
	        self.tabList.SetSelectIndex(0, true);
	        
	        self.attrList.SetData(new List<int>()
	        {
		        NumericType.HeathMax,
		        NumericType.AttackSpeed,
		        NumericType.Power,
		        NumericType.Intellect,
		        NumericType.Physique,
		        NumericType.Agile,
		        NumericType.Insight,
	        });
	        foreach (var ui in self.equipUI)
	        {
		        ui.Value.SetData(ui.Key);
	        }
        }
        
        public static void OnRemove(this UIBagComponent self)
        {
	        SignalHelper.Remove(self.DomainScene(), SignalKey.UI_NumericChanged, self, self.OnNumericChange);
	        SignalHelper.Remove<int>(self.DomainScene(), SignalKey.UI_EquipmentChanged_Hole, self, self.OnEquipmentChange);
        }
	}
}

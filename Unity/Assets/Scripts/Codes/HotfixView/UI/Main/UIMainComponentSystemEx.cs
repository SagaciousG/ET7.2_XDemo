
using System.Collections.Generic;
using UnityEngine;
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UIMainComponent))]
	public static class UIMainComponentSystem
	{
        public static void OnAwake(this UIMainComponent self)
        {
	        self.bag.OnClick(self.OnBagClick);
	        self.skill.OnClick(self.OnSkillClick);
	        self.dialog.OnClick(self.OnDialog);
	        self.opList.gameObject.GetComponent<UIClickInRect>().OnHide += self.OnOpListHide;

	        self.opList.OnData += self.OnOpListData;
	        self.opList.OnSelectedViewRefresh += self.OnOpListSelected;
	        
	        self.interactList.OnData += self.OnInteractListData;
	        self.interactList.OnSelectedViewRefresh += self.OnInteractListSelected;
        }

        private static async void OnInteractListSelected(this UIMainComponent self, RectTransform cell, bool selected, object data,
	        int index)
        {
	        var op = (int)data;
	        var cfg = OptionCodeConfigCategory.Instance.Get(op);
	        var response = await SessionHelper.Call<InteractToResponse>(self.ClientScene(), new InteractToRequest()
	        {
		        Option = (int)op,
		        To = self.InteractTo.Id,
	        });
	        self.interactList.gameObject.Display(false);
	        switch (cfg.FieldName)
	        {
		        case OptionCode.LotterySkill:
		        {
			        UIHelper.Create(UIType.UILotterySkill, self.DomainScene()).Coroutine();
			        break;
		        }
		        case OptionCode.LotteryEquip:
		        {
			        UIHelper.Create(UIType.UILotteryEquip, self.DomainScene()).Coroutine();
			        break;
		        }
	        }
        }
        
        private static void OnInteractListData(this UIMainComponent self, int index, RectTransform cell, object data)
        {
	        var option = (int)data;
	        var rc = cell.GetComponent<UIReferenceCollector>();
	        var title = rc.Get<XText>("title");
	        title.text = OptionCodeConfigCategory.Instance.Get(option).Name;
        }
        private static async void OnOpListSelected(this UIMainComponent self, RectTransform cell, bool selected, object data, int index)
        {
	        var unit = (Unit)data;
	        self.InteractTo = unit;
	        if (selected)
	        {
		        var response = await SessionHelper.Call<GetInteractCodesALResponse>(self.ClientScene(),
			        new GetInteractCodesALRequest() { Unit = unit.Id });
		        self.interactList.SetData(response.Options);
		        self.opList.gameObject.Display(false);
		        self.interactList.gameObject.Display(true);
	        }
        }
        
        private static void OnOpListData(this UIMainComponent self, int index, RectTransform cell, object data)
        {
	        var unit = (Unit)data;
	        var rc = cell.GetComponent<UIReferenceCollector>();
	        var title = rc.Get<XText>("title");
	        title.text = unit.Name;
        }

        private static void OnBuildMapClick(this UIMainComponent self)
        {
	        UIHelper.Create(UIType.UIMapStore, self.DomainScene()).Coroutine();
        }
        
        private static void OnSkillClick(this UIMainComponent self)
        {
	        UIHelper.Create(UIType.UISkill, self.DomainScene()).Coroutine();
        }
        
        private static void OnBagClick(this UIMainComponent self)
        {
	        UIHelper.Create(UIType.UIBag, self.DomainScene()).Coroutine();
        }

        private static void OnOpListHide(this UIMainComponent self, UIClickInRect obj)
        {
			self.dialog.Display(self.DialogUnits.Count > 0);    
        }
        
        private static void OnDialog(this UIMainComponent self)
        {
			self.dialog.Display(false);   
			self.RefreshOpList();
        }

        private static void RefreshOpList(this UIMainComponent self)
        {
	        self.opList.gameObject.Display(self.DialogUnits.Count > 0);
	        self.opList.SetData(self.DialogUnits);
        }
        
        public static void SetDialog(this UIMainComponent self, List<Unit> units)
        {
	        self.DialogUnits = units;
	        if (self.opList.gameObject.IsDisplay())
	        {
				self.RefreshOpList();      
	        }
	        else
	        {
				self.dialog.Display(units.Count > 0);
	        }
        }
        public static void OnCreate(this UIMainComponent self)
        {
            
        }
        
        public static void SetData(this UIMainComponent self, params object[] args)
        {
	        UIHelper.CreateUIMoney(self.GetParent<UI>(), MoneyType.Copper).Coroutine();
	        self.dialog.Display(false);
	        self.opList.gameObject.Display(false);
	        self.interactList.gameObject.Display(false);
        }
        
        public static void OnRemove(this UIMainComponent self)
        {
            
        }
	}
}

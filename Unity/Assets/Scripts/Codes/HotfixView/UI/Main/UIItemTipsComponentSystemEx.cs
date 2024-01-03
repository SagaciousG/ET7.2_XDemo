
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
	[FriendOf(typeof(UIItemTipsComponent))]
	public static class UIItemTipsComponentSystem
	{
        public static void OnAwake(this UIItemTipsComponent self)
        {
	        self.optionList.OnData += self.OnOptionData;
	        self.optionList.OnSelectedViewRefresh += self.OnOptionSelected;
        }

        private static async void OnOptionSelected(this UIItemTipsComponent self, RectTransform cell, bool selected,
	        object data, int index)
        {
	        var id = (int)data;
	        var unit = await UnitHelper.GetMyUnitFromCurrentScene(self.DomainScene());
	        var cfg = OptionCodeConfigCategory.Instance.Get(id);
	        switch (cfg.FieldName)
	        {
		        case OptionCode.UseItem:
		        {
			        var response = await SessionHelper.Call<UseItemALResponse>(self.ClientScene(),
				        new UseItemALRequest() { UID = self.ShowData.UID, Num = self.ShowData.Num });
			        if (response.Error > 0)
			        {
				        UIHelper.PopError(self.DomainScene(), response.Error, response.Message);
				        return;
			        }
			        self.Close();
			        break;
		        }
		        case OptionCode.EquipUp:
		        {
			        var itemCfg = ItemConfigCategory.Instance.Get(self.ShowData.Id);
			        var part = (EquipmentPart)itemCfg.GetArm().Part;
			        var response = await SessionHelper.Call<EquipUpArmsALResponse>(self.ClientScene(),
				        new EquipUpArmsALRequest()
				        {
					        Hole = (int)part.ToHole(),
					        UID = self.ShowData.UID,
					        Profession = (int)unit.ProfessionNum
				        });
			        if (response.Error > 0)
			        {
				        UIHelper.PopError(self.DomainScene(), response.Error, response.Message);
				        return;
			        }
			        self.Close();
			        break;
		        }
		        case OptionCode.EquipDown:
		        {
			        var response = await SessionHelper.Call<EquipDownArmsALResponse>(self.ClientScene(),
				        new EquipDownArmsALRequest()
				        {
					        UID = self.ShowData.UID,
					        Profession = (int)unit.ProfessionNum
				        });
			        if (response.Error > 0)
			        {
				        UIHelper.PopError(self.DomainScene(), response.Error, response.Message);
				        return;
			        }
			        self.Close();
			        break;
		        }
	        }
        }
        private static void OnOptionData(this UIItemTipsComponent self, int index, RectTransform cell, object data)
        {
	        var id = (int)data;
	        var cfg = OptionCodeConfigCategory.Instance.Get(id);
	        var rc = cell.GetComponent<UIReferenceCollector>();
	        self.title = rc.Get<XText>("title");
	        self.title.text = cfg.Name;
        }

        public static void OnCreate(this UIItemTipsComponent self)
        {
            UIHelper.CreateUIBgClose(self.GetParent<UI>()).Coroutine();
        }
        
        public static async void SetData(this UIItemTipsComponent self, params object[] args)
        {
	        self.ShowData = (UIBagItemArgs)args[0];
	        
	        var cfg = ItemConfigCategory.Instance.Get(self.ShowData.Id);
	        self.iconUI.SetData(self.ShowData);
	        self.title.text = cfg.Name;
	        self.descMain.text = cfg.Desc;
	        self.descSub.text = "";
	        switch ((BagItemType)cfg.Type)
	        {
		        case BagItemType.Arms:
		        {
			        var arm = ItemArmConfigCategory.Instance.Get(cfg.Index);
			        self.subTitle.text = $"部位：{((UnitPartType)arm.Part).GetNameAttribute().Name}";
			        var equipCont = await UIHelper.CreateSingleUI(self.GetParent<UI>(), UIType.UIEquipAttrCont, self.content);
			        equipCont.SetData(cfg);
			        break;
		        }
		        case BagItemType.Consume:
		        {
			        self.subTitle.text = $"数量：{self.ShowData.Num}";
			        break;
		        }
	        }

	        var options = cfg.OptionBtn.ToIntArray(',');
	        var showOptions = new List<int>();
	        foreach (var option in options)
	        {
		        var optionCodeConfig = OptionCodeConfigCategory.Instance.Get(option);
		        if (optionCodeConfig.ShowCondition == (int)self.ShowData.ShowState)
		        {
			        showOptions.Add(option);
		        }
	        }
	        self.optionList.SetData(showOptions);
        }
        
        public static void OnRemove(this UIItemTipsComponent self)
        {
            
        }
	}
}

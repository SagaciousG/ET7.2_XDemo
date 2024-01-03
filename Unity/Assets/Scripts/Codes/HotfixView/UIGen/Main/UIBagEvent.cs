using UnityEngine.UI;
using UnityEngine;
using ET;
using System.Collections.Generic;


namespace ET.Client
{
    [UIEvent(UIType.UIBag)]
    [FriendOf(typeof(UIBagComponent))]
    public class UIBagEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIBagComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.itemList = rc.Get<UIList>("itemList");
			self.tabList = rc.Get<UIList>("tabList");
			self.profession0 = rc.Get<RectTransform>("profession0");
			self.profession1 = rc.Get<RectTransform>("profession1");
			self.equip_0 = rc.Get<XSubUI>("equip_0");
			self.attrList = rc.Get<UIList>("attrList");
			self.equip_1 = rc.Get<XSubUI>("equip_1");
			self.equip_2 = rc.Get<XSubUI>("equip_2");
			self.equip_3 = rc.Get<XSubUI>("equip_3");
			self.equip_4 = rc.Get<XSubUI>("equip_4");
			self.equip_5 = rc.Get<XSubUI>("equip_5");
			self.equip_6 = rc.Get<XSubUI>("equip_6");
			self.equip_7 = rc.Get<XSubUI>("equip_7");
			self.equip.Add(0, self.equip_0);
			self.equip.Add(1, self.equip_1);
			self.equip.Add(2, self.equip_2);
			self.equip.Add(3, self.equip_3);
			self.equip.Add(4, self.equip_4);
			self.equip.Add(5, self.equip_5);
			self.equip.Add(6, self.equip_6);
			self.equip.Add(7, self.equip_7);
			self.profession.Add(0, self.profession0);
			self.profession.Add(1, self.profession1);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.itemList);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.tabList);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.attrList);
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIBagComponent self)
        {
			self.equip_0UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.equip_0.uiPrefab.name, self.equip_0.transform);
			self.equip_1UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.equip_1.uiPrefab.name, self.equip_1.transform);
			self.equip_2UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.equip_2.uiPrefab.name, self.equip_2.transform);
			self.equip_3UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.equip_3.uiPrefab.name, self.equip_3.transform);
			self.equip_4UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.equip_4.uiPrefab.name, self.equip_4.transform);
			self.equip_5UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.equip_5.uiPrefab.name, self.equip_5.transform);
			self.equip_6UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.equip_6.uiPrefab.name, self.equip_6.transform);
			self.equip_7UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.equip_7.uiPrefab.name, self.equip_7.transform);
			self.equipUI.Add(0, self.equip_0UI);
			self.equipUI.Add(1, self.equip_1UI);
			self.equipUI.Add(2, self.equip_2UI);
			self.equipUI.Add(3, self.equip_3UI);
			self.equipUI.Add(4, self.equip_4UI);
			self.equipUI.Add(5, self.equip_5UI);
			self.equipUI.Add(6, self.equip_6UI);
			self.equipUI.Add(7, self.equip_7UI);

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIBagComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIBagComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIBagComponent)ui.Component).OnRemove();
        }
    }
}
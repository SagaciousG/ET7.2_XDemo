using UnityEngine.UI;
using UnityEngine;
using ET;
using System.Collections.Generic;


namespace ET.Client
{
    [UIEvent(UIType.UITabDemo)]
    [FriendOf(typeof(UITabDemoComponent))]
    public class UITabDemoEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UITabDemoComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.t1 = rc.Get<XUITab>("t1");
			self.t2 = rc.Get<XUITab>("t2");
			self.t3 = rc.Get<XUITab>("t3");
			self.t4 = rc.Get<XUITab>("t4");
			self.t.Add(1, self.t1);
			self.t.Add(2, self.t2);
			self.t.Add(3, self.t3);
			self.t.Add(4, self.t4);
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UITabDemoComponent self)
        {
			self.t1.OnCreateInstance += async (a) =>
			{
				self.t1UI = await UIHelper.BindSingleUI(self.GetParent<UI>(), self.t1.uiPrefab.name, a);
				self.t1UI.SetData();
			};
			self.t2.OnCreateInstance += async (a) =>
			{
				self.t2UI = await UIHelper.BindSingleUI(self.GetParent<UI>(), self.t2.uiPrefab.name, a);
				self.t2UI.SetData();
			};
			self.t3.OnCreateInstance += async (a) =>
			{
				self.t3UI = await UIHelper.BindSingleUI(self.GetParent<UI>(), self.t3.uiPrefab.name, a);
				self.t3UI.SetData();
			};
			self.t4.OnCreateInstance += async (a) =>
			{
				self.t4UI = await UIHelper.BindSingleUI(self.GetParent<UI>(), self.t4.uiPrefab.name, a);
				self.t4UI.SetData();
			};

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UITabDemoComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UITabDemoComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UITabDemoComponent)ui.Component).OnRemove();
        }
    }
}
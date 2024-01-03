using UnityEngine.UI;
using UnityEngine;
using ET;
using System.Collections.Generic;


namespace ET.Client
{
    [UIEvent(UIType.UICoinBanner)]
    [FriendOf(typeof(UICoinBannerComponent))]
    public class UICoinBannerEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UICoinBannerComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.content = rc.Get<RectTransform>("content");
			self.item1 = rc.Get<XSubUI>("item1");
			self.item2 = rc.Get<XSubUI>("item2");
			self.item.Add(1, self.item1);
			self.item.Add(2, self.item2);
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UICoinBannerComponent self)
        {
			self.item1UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.item1.uiPrefab.name, self.item1.transform);
			self.item2UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.item2.uiPrefab.name, self.item2.transform);
			self.itemUI.Add(1, self.item1UI);
			self.itemUI.Add(2, self.item2UI);

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UICoinBannerComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UICoinBannerComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UICoinBannerComponent)ui.Component).OnRemove();
        }
    }
}
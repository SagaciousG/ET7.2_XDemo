using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UIItemTips)]
    [FriendOf(typeof(UIItemTipsComponent))]
    public class UIItemTipsEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIItemTipsComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.icon = rc.Get<XSubUI>("icon");
			self.title = rc.Get<XText>("title");
			self.subTitle = rc.Get<XText>("subTitle");
			self.expiryDate = rc.Get<RectTransform>("expiryDate");
			self.desc = rc.Get<XImage>("desc");
			self.descMain = rc.Get<XText>("descMain");
			self.descSub = rc.Get<XText>("descSub");
			self.optionList = rc.Get<UIList>("optionList");
			self.content = rc.Get<RectTransform>("content");
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.optionList);
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIItemTipsComponent self)
        {
			self.iconUI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.icon.uiPrefab.name, self.icon.transform);

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIItemTipsComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIItemTipsComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIItemTipsComponent)ui.Component).OnRemove();
        }
    }
}
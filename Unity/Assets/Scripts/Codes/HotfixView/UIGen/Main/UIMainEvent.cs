using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UIMain)]
    [FriendOf(typeof(UIMainComponent))]
    public class UIMainEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIMainComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.bag = rc.Get<EmptyGraphic>("bag");
			self.dialog = rc.Get<XImage>("dialog");
			self.opList = rc.Get<UIList>("opList");
			self.skill = rc.Get<EmptyGraphic>("skill");
			self.interactList = rc.Get<UIList>("interactList");
			self.exp = rc.Get<RectTransform>("exp");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIMainComponent self)
        {
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.opList);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.interactList);

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIMainComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIMainComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIMainComponent)ui.Component).OnRemove();
        }
    }
}
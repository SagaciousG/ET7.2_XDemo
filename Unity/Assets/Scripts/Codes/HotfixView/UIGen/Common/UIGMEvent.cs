using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;
using System.Collections.Generic;


namespace ET.Client
{
    [UIEvent(UIType.UIGM)]
    [FriendOf(typeof(UIGMComponent))]
    public class UIGMEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIGMComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.submit = rc.Get<XImage>("submit");
			self.pt3 = rc.Get<TextMeshProUGUI>("pt3");
			self.pt2 = rc.Get<TextMeshProUGUI>("pt2");
			self.pt1 = rc.Get<TextMeshProUGUI>("pt1");
			self.popList = rc.Get<UIList>("popList");
			self.param3 = rc.Get<RectTransform>("param3");
			self.param2 = rc.Get<RectTransform>("param2");
			self.param1 = rc.Get<RectTransform>("param1");
			self.p3 = rc.Get<XInputField>("p3");
			self.p2 = rc.Get<XInputField>("p2");
			self.p1 = rc.Get<XInputField>("p1");
			self.drop3 = rc.Get<XPopup>("drop3");
			self.drop2 = rc.Get<XPopup>("drop2");
			self.drop1 = rc.Get<XPopup>("drop1");
			self.desc = rc.Get<TextMeshProUGUI>("desc");
			self.commonList = rc.Get<UIList>("commonList");
			self.close = rc.Get<XImage>("close");
			self.allList = rc.Get<UIList>("allList");
			self.drop.Add(3, self.drop3);
			self.drop.Add(2, self.drop2);
			self.drop.Add(1, self.drop1);
			self.p.Add(3, self.p3);
			self.p.Add(2, self.p2);
			self.p.Add(1, self.p1);
			self.param.Add(3, self.param3);
			self.param.Add(2, self.param2);
			self.param.Add(1, self.param1);
			self.pt.Add(3, self.pt3);
			self.pt.Add(2, self.pt2);
			self.pt.Add(1, self.pt1);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.popList);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.commonList);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.allList);
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIGMComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIGMComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIGMComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIGMComponent)ui.Component).OnRemove();
        }
    }
}
using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;
using System.Collections.Generic;


namespace ET.Client
{
    [UIEvent(UIType.UIPopTips)]
    [FriendOf(typeof(UIPopTipsComponent))]
    public class UIPopTipsEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIPopTipsComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.txt3 = rc.Get<TextMeshProUGUI>("txt3");
			self.tips3 = rc.Get<RectTransform>("tips3");
			self.tips2 = rc.Get<RectTransform>("tips2");
			self.tips1 = rc.Get<RectTransform>("tips1");
			self.content = rc.Get<RectTransform>("content");
			self.txt1 = rc.Get<TextMeshProUGUI>("txt1");
			self.txt2 = rc.Get<TextMeshProUGUI>("txt2");
			self.tips.Add(3, self.tips3);
			self.tips.Add(2, self.tips2);
			self.tips.Add(1, self.tips1);
			self.txt.Add(3, self.txt3);
			self.txt.Add(1, self.txt1);
			self.txt.Add(2, self.txt2);
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIPopTipsComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIPopTipsComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIPopTipsComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIPopTipsComponent)ui.Component).OnRemove();
        }
    }
}
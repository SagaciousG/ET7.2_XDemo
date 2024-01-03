using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
    [UIEvent(UIType.UIDialogEx)]
    [FriendOf(typeof(UIDialogExComponent))]
    public class UIDialogExEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIDialogExComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.title = rc.Get<TextMeshProUGUI>("title");
			self.desc = rc.Get<TextMeshProUGUI>("desc");
			self.leftBtn = rc.Get<XImage>("leftBtn");
			self.leftText = rc.Get<TextMeshProUGUI>("leftText");
			self.rightBtn = rc.Get<XImage>("rightBtn");
			self.rightText = rc.Get<TextMeshProUGUI>("rightText");
			self.centerBtn = rc.Get<XImage>("centerBtn");
			self.centerText = rc.Get<TextMeshProUGUI>("centerText");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIDialogExComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIDialogExComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIDialogExComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIDialogExComponent)ui.Component).OnRemove();
        }
    }
}
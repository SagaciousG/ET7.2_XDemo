using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
    [UIEvent(UIType.UILogin)]
    [FriendOf(typeof(UILoginComponent))]
    public class UILoginEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UILoginComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.tips = rc.Get<TextMeshProUGUI>("tips");
			self.account = rc.Get<XInputField>("account");
			self.password = rc.Get<XInputField>("password");
			self.loginBtn = rc.Get<XImage>("loginBtn");
			self.registerBtn = rc.Get<XImage>("registerBtn");
			self.tabBtn = rc.Get<XImage>("tabBtn");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UILoginComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UILoginComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UILoginComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UILoginComponent)ui.Component).OnRemove();
        }
    }
}
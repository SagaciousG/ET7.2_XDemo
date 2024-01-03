using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
    [UIEvent(UIType.UIDialogWithInput)]
    [FriendOf(typeof(UIDialogWithInputComponent))]
    public class UIDialogWithInputEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIDialogWithInputComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.title = rc.Get<TextMeshProUGUI>("title");
			self.desc = rc.Get<TextMeshProUGUI>("desc");
			self.leftBtn = rc.Get<XImage>("leftBtn");
			self.leftText = rc.Get<TextMeshProUGUI>("leftText");
			self.rightBtn = rc.Get<XImage>("rightBtn");
			self.rightText = rc.Get<TextMeshProUGUI>("rightText");
			self.input = rc.Get<XInputField>("input");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIDialogWithInputComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIDialogWithInputComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIDialogWithInputComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIDialogWithInputComponent)ui.Component).OnRemove();
        }
    }
}
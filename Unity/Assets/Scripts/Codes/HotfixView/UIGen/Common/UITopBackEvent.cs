using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
    [UIEvent(UIType.UITopBack)]
    [FriendOf(typeof(UITopBackComponent))]
    public class UITopBackEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UITopBackComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.back = rc.Get<XImage>("back");
			self.title = rc.Get<TextMeshProUGUI>("title");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UITopBackComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UITopBackComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UITopBackComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UITopBackComponent)ui.Component).OnRemove();
        }
    }
}
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UITopCoinItem)]
    [FriendOf(typeof(UITopCoinItemComponent))]
    public class UITopCoinItemEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UITopCoinItemComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.coin = rc.Get<XImage>("coin");
			self.num = rc.Get<XText>("num");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UITopCoinItemComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UITopCoinItemComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UITopCoinItemComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UITopCoinItemComponent)ui.Component).OnRemove();
        }
    }
}
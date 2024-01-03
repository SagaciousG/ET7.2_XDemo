using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UIBagItem)]
    [FriendOf(typeof(UIBagItemComponent))]
    public class UIBagItemEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIBagItemComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.icon = rc.Get<XImage>("icon");
			self.num = rc.Get<XText>("num");
			self.quality = rc.Get<XImage>("quality");
			self.frame = rc.Get<XImage>("frame");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIBagItemComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIBagItemComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIBagItemComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIBagItemComponent)ui.Component).OnRemove();
        }
    }
}
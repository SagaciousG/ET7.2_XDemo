using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UIAttrItem)]
    [FriendOf(typeof(UIAttrItemComponent))]
    public class UIAttrItemEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIAttrItemComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.key = rc.Get<XText>("key");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIAttrItemComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIAttrItemComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIAttrItemComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIAttrItemComponent)ui.Component).OnRemove();
        }
    }
}
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UIGMBar)]
    [FriendOf(typeof(UIGMBarComponent))]
    public class UIGMBarEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIGMBarComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.node = rc.Get<RectTransform>("node");
			self.open = rc.Get<XImage>("open");
			self.content = rc.Get<RectTransform>("content");
			self.dragArea = rc.Get<XDragArea>("dragArea");
			self.gm = rc.Get<XImage>("gm");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIGMBarComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIGMBarComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIGMBarComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIGMBarComponent)ui.Component).OnRemove();
        }
    }
}
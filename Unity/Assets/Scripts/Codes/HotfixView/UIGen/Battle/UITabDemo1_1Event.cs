using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UITabDemo1_1)]
    [FriendOf(typeof(UITabDemo1_1Component))]
    public class UITabDemo1_1Event: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UITabDemo1_1Component)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.t = rc.Get<XText>("t");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UITabDemo1_1Component self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UITabDemo1_1Component)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UITabDemo1_1Component)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UITabDemo1_1Component)ui.Component).OnRemove();
        }
    }
}
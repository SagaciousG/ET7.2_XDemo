using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UITabDemo1_3)]
    [FriendOf(typeof(UITabDemo1_3Component))]
    public class UITabDemo1_3Event: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UITabDemo1_3Component)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.t = rc.Get<XText>("t");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UITabDemo1_3Component self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UITabDemo1_3Component)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UITabDemo1_3Component)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UITabDemo1_3Component)ui.Component).OnRemove();
        }
    }
}
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UIClickBgClose)]
    [FriendOf(typeof(UIClickBgCloseComponent))]
    public class UIClickBgCloseEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIClickBgCloseComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.bg = rc.Get<XImage>("bg");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIClickBgCloseComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIClickBgCloseComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIClickBgCloseComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIClickBgCloseComponent)ui.Component).OnRemove();
        }
    }
}
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UIItemPopup)]
    [FriendOf(typeof(UIItemPopupComponent))]
    public class UIItemPopupEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIItemPopupComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();

  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIItemPopupComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIItemPopupComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIItemPopupComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIItemPopupComponent)ui.Component).OnRemove();
        }
    }
}
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UIHeaderContainer)]
    [FriendOf(typeof(UIHeaderContainerComponent))]
    public class UIHeaderContainerEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIHeaderContainerComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIHeaderContainerComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIHeaderContainerComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIHeaderContainerComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIHeaderContainerComponent)ui.Component).OnRemove();
        }
    }
}
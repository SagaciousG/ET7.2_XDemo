using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
    [UIEvent(UIType.UIHeader)]
    [FriendOf(typeof(UIHeaderComponent))]
    public class UIHeaderEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIHeaderComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.name = rc.Get<TextMeshProUGUI>("name");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIHeaderComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIHeaderComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIHeaderComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIHeaderComponent)ui.Component).OnRemove();
        }
    }
}
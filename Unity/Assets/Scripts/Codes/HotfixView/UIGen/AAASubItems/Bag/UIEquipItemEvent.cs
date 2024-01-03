using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UIEquipItem)]
    [FriendOf(typeof(UIEquipItemComponent))]
    public class UIEquipItemEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIEquipItemComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.bagItem = rc.Get<XSubUI>("bagItem");
			self.icon = rc.Get<XImage>("icon");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIEquipItemComponent self)
        {
			self.bagItemUI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.bagItem.uiPrefab.name, self.bagItem.transform);

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIEquipItemComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIEquipItemComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIEquipItemComponent)ui.Component).OnRemove();
        }
    }
}
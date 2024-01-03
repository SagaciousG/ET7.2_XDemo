using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UILotteryEquip)]
    [FriendOf(typeof(UILotteryEquipComponent))]
    public class UILotteryEquipEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UILotteryEquipComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.ten = rc.Get<XImage>("ten");
			self.one = rc.Get<XImage>("one");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UILotteryEquipComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UILotteryEquipComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UILotteryEquipComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UILotteryEquipComponent)ui.Component).OnRemove();
        }
    }
}
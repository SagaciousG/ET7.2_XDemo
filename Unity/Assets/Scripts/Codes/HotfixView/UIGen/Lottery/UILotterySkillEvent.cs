using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UILotterySkill)]
    [FriendOf(typeof(UILotterySkillComponent))]
    public class UILotterySkillEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UILotterySkillComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.ten = rc.Get<XImage>("ten");
			self.one = rc.Get<XImage>("one");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UILotterySkillComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UILotterySkillComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UILotterySkillComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UILotterySkillComponent)ui.Component).OnRemove();
        }
    }
}
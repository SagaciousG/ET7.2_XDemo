using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.UISkill)]
    [FriendOf(typeof(UISkillComponent))]
    public class UISkillEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UISkillComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.skillList = rc.Get<UIList>("skillList");
			self.name = rc.Get<XText>("name");
			self.levelUpper = rc.Get<RectTransform>("levelUpper");
			self.upperNum = rc.Get<XText>("upperNum");
			self.desc = rc.Get<XText>("desc");
			self.descNext = rc.Get<XText>("descNext");
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.skillList);
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UISkillComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UISkillComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UISkillComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UISkillComponent)ui.Component).OnRemove();
        }
    }
}
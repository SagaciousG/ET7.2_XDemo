using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.SelectRoleCell)]
    [FriendOf(typeof(SelectRoleCellComponent))]
    public class SelectRoleCellEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (SelectRoleCellComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.select = rc.Get<XImage>("select");
			self.role = rc.Get<XModelImage>("role");
			self.name = rc.Get<XText>("name");
			self.clickArea = rc.Get<EmptyGraphic>("clickArea");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(SelectRoleCellComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((SelectRoleCellComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((SelectRoleCellComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((SelectRoleCellComponent)ui.Component).OnRemove();
        }
    }
}
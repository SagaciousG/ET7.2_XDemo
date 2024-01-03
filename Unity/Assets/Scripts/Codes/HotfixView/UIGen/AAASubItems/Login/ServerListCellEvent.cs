using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.ServerListCell)]
    [FriendOf(typeof(ServerListCellComponent))]
    public class ServerListCellEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (ServerListCellComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.bg = rc.Get<XImage>("bg");
			self.server = rc.Get<XText>("server");
			self.state = rc.Get<XImage>("state");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(ServerListCellComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((ServerListCellComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((ServerListCellComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((ServerListCellComponent)ui.Component).OnRemove();
        }
    }
}
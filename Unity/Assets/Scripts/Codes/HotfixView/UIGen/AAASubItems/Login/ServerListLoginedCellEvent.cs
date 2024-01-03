using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
    [UIEvent(UIType.ServerListLoginedCell)]
    [FriendOf(typeof(ServerListLoginedCellComponent))]
    public class ServerListLoginedCellEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (ServerListLoginedCellComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.bg = rc.Get<XImage>("bg");
			self.title = rc.Get<XText>("title");
			self.server = rc.Get<XText>("server");
			self.state = rc.Get<XImage>("state");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(ServerListLoginedCellComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((ServerListLoginedCellComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((ServerListLoginedCellComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((ServerListLoginedCellComponent)ui.Component).OnRemove();
        }
    }
}
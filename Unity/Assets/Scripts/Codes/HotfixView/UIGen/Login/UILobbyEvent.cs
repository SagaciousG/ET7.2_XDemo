using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
    [UIEvent(UIType.UILobby)]
    [FriendOf(typeof(UILobbyComponent))]
    public class UILobbyEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UILobbyComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.selectServer = rc.Get<XImage>("selectServer");
			self.serverName = rc.Get<TextMeshProUGUI>("serverName");
			self.serverState = rc.Get<XImage>("serverState");
			self.serverPanel = rc.Get<RectTransform>("serverPanel");
			self.serverPanelBg = rc.Get<XImage>("serverPanelBg");
			self.bg = rc.Get<XImage>("bg");
			self.serverPanelClose = rc.Get<XImage>("serverPanelClose");
			self.tuijian = rc.Get<XImage>("tuijian");
			self.tabList = rc.Get<UIList>("tabList");
			self.tuijianCont = rc.Get<RectTransform>("tuijianCont");
			self.lastList = rc.Get<UIList>("lastList");
			self.tuijianList = rc.Get<UIList>("tuijianList");
			self.serverList = rc.Get<UIList>("serverList");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UILobbyComponent self)
        {
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.tabList);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.lastList);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.tuijianList);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.serverList);

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UILobbyComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UILobbyComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UILobbyComponent)ui.Component).OnRemove();
        }
    }
}
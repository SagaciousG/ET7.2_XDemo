
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UILobbyComponent))]
	public static class UILobbyComponentSystem
	{
        public static void OnAwake(this UILobbyComponent self)
        {
	        self.serverPanel.Display(false);
	        self.bg.OnClick(() =>
	        {
		        self.EnterZone().Coroutine();
	        });
	        self.selectServer.OnClick(() =>
	        {
		        self.serverPanel.Display(true);
	        });
	        self.serverPanelClose.OnClick(() =>
	        {
		        self.serverPanel.Display(false);
	        });
	        self.tuijian.OnClick(self.OnTuiJian);

	        self.tabList.OnData += self.TabListOnData;
	        self.tabList.OnSelectedViewRefresh += self.TabListOnSelected;
	        self.lastList.OnSelectedViewRefresh += self.LastListOnSelected;
	        
	        self.serverList.OnSelectedViewRefresh += self.AllServerList_OnSelected;
        }

        private static void OnTuiJian(this UILobbyComponent self)
        {
	        self.tabList.SetSelectIndex(-1);
	        self.tuijianCont.Display(true);
	        self.serverList.gameObject.Display(false);
        }
        
        private static void LastListOnSelected(this UILobbyComponent self, RectTransform cell, bool selected, object data,
	        int index)
        {
	        if (selected)
	        {
		        var cfg = ZoneConfigCategory.Instance.Get(self.LatestEnterZones[index]);
		        self.OnSelected(cfg);
	        }
        }

        private static void TabListOnData(this UILobbyComponent self, int index, RectTransform cell, object data)
        {
	        var val = (int)data;
	        var rc = cell.GetComponent<UIReferenceCollector>();
	        var title = rc.Get<XText>("title");
	        title.text = $"{(val - 1) * 10 + 1}-{val * 10}服";
        }

        private static void TabListOnSelected(this UILobbyComponent self, RectTransform cell, bool selected, object data,
	        int index)
        {
	        var rc = cell.GetComponent<UIReferenceCollector>();
	        var select = rc.Get<XImage>("select");
	        select.Display(selected);
	        if (selected)
	        {
		        self.tuijianCont.Display(false);
		        self.serverList.gameObject.Display(true);
		        self.serverList.SetData(self.Tabs[(int)data]);
	        }
        }

        private static void OnSelected(this UILobbyComponent self, ZoneConfig cfg)
        {
	        self.SelectedZone = cfg.Id;
	        self.serverName.text = cfg.ShowName;
	        self.serverPanel.Display(false);
	        if (self.OnlineZones.TryGetValue(cfg.Id, out var zoneInfo))
	        {
		        var playerCount = zoneInfo.PlayerCount;
		        if (playerCount > 1000)
			        self.serverState.Skin = "fs_common_zy_hong";
		        else if (playerCount > 500)
			        self.serverState.Skin = "fs_common_zy_jihuo";
		        else
			        self.serverState.Skin = "fs_common_zy_lv";
	        }
	        else
	        {
		        self.serverState.Skin = "fs_common_zy_weijihuo";
	        }
        }

        private static void AllServerList_OnSelected(this UILobbyComponent self, RectTransform cell, bool selected, object data, int index)
        {
	        var cfg = (ZoneConfig)data;
			self.OnSelected(cfg);
        }
        
        public static void OnCreate(this UILobbyComponent self)
        {
            
        }

        public static void SetData(this UILobbyComponent self, params object[] args)
        {
	        var zoneListComponent = self.DomainScene().GetComponent<PlayerComponent>().GetComponent<PlayerZoneListComponent>();
	        self.OnlineZones = new Dictionary<int, ZoneInfo>();
	        foreach (var zone in zoneListComponent.ZoneInfos)
	        {
		        self.OnlineZones[zone.Zone] = zone;
	        }

	        var all = ZoneConfigCategory.Instance.GetAll();
	        self.Tabs = new MultiMap<int, ZoneConfig>();
	        var index = 1;
	        foreach (var kv in all)
	        {
		        if (kv.Value.Hide == 1)
			        continue;
		        self.Tabs.Add(Mathf.CeilToInt(index / 10f), kv.Value);
		        index++;
	        }
	        self.tabList.SetData(self.Tabs.Keys.ToArray());
	        var latestEnterZones = zoneListComponent.LatestEnterZone;
	        self.LatestEnterZones = latestEnterZones;
	        if (latestEnterZones?.Count > 0)
	        {
		        self.SelectedZone = latestEnterZones[0];
		        var zoneConfig = ZoneConfigCategory.Instance.Get(self.SelectedZone);

		        self.serverName.text = zoneConfig.ShowName;
		        self.serverPanel.Display(false);
		        if (self.OnlineZones.TryGetValue(zoneConfig.Id, out var zoneInfo))
		        {
			        var playerCount = zoneInfo.PlayerCount;
			        if (playerCount > 1000)
				        self.serverState.Skin = "common_dot_red";
			        else if (playerCount > 500)
				        self.serverState.Skin = "common_dot_yellow";
			        else
				        self.serverState.Skin = "common_dot_green";
		        }
		        else
		        {
			        self.serverState.Skin = "common_dot_gray";
			        
		        }
	        }
	        else
	        {
		        self.serverName.text = "选择服务器";
			    self.serverState.Skin = "common_dot_gray";
	        }
        }
        
        public static void OnRemove(this UILobbyComponent self)
        {
            
        }
        
        public static async ETTask EnterZone(this UILobbyComponent self)
        {
	        if (self.SelectedZone == 0)
	        {
		        self.serverPanel.Display(true);
		        return;
	        }

	        await LobbyHelper.EnterZone(self.ClientScene(), self.SelectedZone);
	        
	        await UIHelper.Create(UIType.UISelectRole, self.DomainScene(), UILayer.Mid);
	        await UIHelper.Remove(UIType.UILobby, self.DomainScene());
        }
	}
}

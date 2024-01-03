
namespace ET.Client
{
    [FriendOf(typeof(ServerListCellComponent))]
	public static class ServerListCellComponentSystem
	{
        public static void OnAwake(this ServerListCellComponent self)
        {
        }

        public static void OnCreate(this ServerListCellComponent self)
        {
            
        }
        
        public static void SetData(this ServerListCellComponent self, params object[] args)
        {
	        var cfg = (ZoneConfig)args[0];
	        self.server.text = cfg.ShowName;
	        var lobbyComponent = self.FindInParent<UILobbyComponent>(UIType.UILobby);
	        if (lobbyComponent.OnlineZones.TryGetValue(cfg.Id, out var zoneInfo))
	        {
		        var playerCount = zoneInfo.PlayerCount;
		        if (playerCount > 1000)
			        self.state.Skin = "common_dot_red";
		        else if (playerCount > 500)
			        self.state.Skin = "common_dot_yellow";
		        else
			        self.state.Skin = "common_dot_green";
	        }
	        else
	        {
		        self.state.Skin = "common_dot_gray";
	        }
        }
        
        public static void OnRemove(this ServerListCellComponent self)
        {
            
        }
	}
}

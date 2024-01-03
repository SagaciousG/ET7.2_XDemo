
namespace ET.Client
{
    [FriendOf(typeof(ServerListLoginedCellComponent))]
	public static class ServerListLoginedCellComponentSystem
	{
        public static void OnAwake(this ServerListLoginedCellComponent self)
        {
	        
        }

        public static void OnCreate(this ServerListLoginedCellComponent self)
        {
            
        }
        
        public static void SetData(this ServerListLoginedCellComponent self, params object[] args)
        {
	        var cfg = (ZoneConfig)args[0];
	        self.server.text = cfg.ShowName;
	        var lobbyComponent = self.GetParent<UI>().GetComponent<UILobbyComponent>();
	        if (lobbyComponent.OnlineZones.TryGetValue(cfg.Id, out var zoneInfo))
	        {
		        var playerCount = zoneInfo.PlayerCount;
		        if (playerCount > 1000)
			        self.state.Skin = "dot_red";
		        else if (playerCount > 500)
			        self.state.Skin = "dot_yellow";
		        else
			        self.state.Skin = "dot_green";
	        }
	        else
	        {
		        self.state.Skin = "dot_gray";
		        self.title.text = $"{zoneInfo.RoleName} Lv.{zoneInfo.RoleLevel}";
	        }
        }
        
        public static void OnRemove(this ServerListLoginedCellComponent self)
        {
            
        }
	}
}

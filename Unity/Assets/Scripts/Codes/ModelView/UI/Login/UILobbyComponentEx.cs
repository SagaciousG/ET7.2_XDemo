using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	public partial class UILobbyComponent
	{
		public Dictionary<int, ZoneInfo> OnlineZones { get; set; }
		public MultiMap<int, ZoneConfig> Tabs;

		public int SelectedZone { set; get; }
		public List<int> LatestEnterZones { get; set; }
	}
}

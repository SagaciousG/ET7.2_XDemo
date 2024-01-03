using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	public partial class UIMapStoreComponent
	{
		public List<GridMapProto> MyMaps = new();
		public List<GridMapProto> StoreMaps = new();

		public int MyPage;
		public int StorePage;
	}
}

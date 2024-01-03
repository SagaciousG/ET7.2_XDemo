//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using System.Collections.Generic;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UISelectRoleComponent : Entity, IAwake, IUIComponent
	{
		public XSubUI role_3;
		public XSubUI role_1;
		public XSubUI role_2;
		public XImage enterMap;
		public UI role_3UI;
		public UI role_1UI;
		public UI role_2UI;
		public Dictionary<int, XSubUI> role = new();
		public Dictionary<int, UI> roleUI = new();

	}
}

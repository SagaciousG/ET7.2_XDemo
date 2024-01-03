//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using System.Collections.Generic;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIBagComponent : Entity, IAwake, IUIComponent
	{
		public UIList itemList;
		public UIList tabList;
		public RectTransform profession0;
		public RectTransform profession1;
		public XSubUI equip_0;
		public UIList attrList;
		public XSubUI equip_1;
		public XSubUI equip_2;
		public XSubUI equip_3;
		public XSubUI equip_4;
		public XSubUI equip_5;
		public XSubUI equip_6;
		public XSubUI equip_7;
		public UI equip_0UI;
		public UI equip_1UI;
		public UI equip_2UI;
		public UI equip_3UI;
		public UI equip_4UI;
		public UI equip_5UI;
		public UI equip_6UI;
		public UI equip_7UI;
		public Dictionary<int, XSubUI> equip = new();
		public Dictionary<int, UI> equipUI = new();
		public Dictionary<int, RectTransform> profession = new();

	}
}

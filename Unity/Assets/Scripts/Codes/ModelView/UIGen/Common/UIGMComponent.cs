//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;
using System.Collections.Generic;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIGMComponent : Entity, IAwake, IUIComponent
	{
		public XImage submit;
		public TextMeshProUGUI pt3;
		public TextMeshProUGUI pt2;
		public TextMeshProUGUI pt1;
		public UIList popList;
		public RectTransform param3;
		public RectTransform param2;
		public RectTransform param1;
		public XInputField p3;
		public XInputField p2;
		public XInputField p1;
		public XPopup drop3;
		public XPopup drop2;
		public XPopup drop1;
		public TextMeshProUGUI desc;
		public UIList commonList;
		public XImage close;
		public UIList allList;
		public Dictionary<int, XPopup> drop = new();
		public Dictionary<int, XInputField> p = new();
		public Dictionary<int, RectTransform> param = new();
		public Dictionary<int, TextMeshProUGUI> pt = new();

	}
}

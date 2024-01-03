//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;
using System.Collections.Generic;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIPopTipsComponent : Entity, IAwake, IUIComponent
	{
		public TextMeshProUGUI txt3;
		public RectTransform tips3;
		public RectTransform tips2;
		public RectTransform tips1;
		public RectTransform content;
		public TextMeshProUGUI txt1;
		public TextMeshProUGUI txt2;
		public Dictionary<int, RectTransform> tips = new();
		public Dictionary<int, TextMeshProUGUI> txt = new();

	}
}

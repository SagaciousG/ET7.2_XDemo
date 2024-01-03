//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using System.Collections.Generic;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UICoinBannerComponent : Entity, IAwake, IUIComponent
	{
		public RectTransform content;
		public XSubUI item1;
		public XSubUI item2;
		public UI item1UI;
		public UI item2UI;
		public Dictionary<int, XSubUI> item = new();
		public Dictionary<int, UI> itemUI = new();

	}
}

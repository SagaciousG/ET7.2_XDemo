//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIGMBarComponent : Entity, IAwake, IUIComponent
	{
		public RectTransform node;
		public XImage open;
		public RectTransform content;
		public XDragArea dragArea;
		public XImage gm;

	}
}

//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIItemTipsComponent : Entity, IAwake, IUIComponent
	{
		public XSubUI icon;
		public XText title;
		public XText subTitle;
		public RectTransform expiryDate;
		public XImage desc;
		public XText descMain;
		public XText descSub;
		public UIList optionList;
		public RectTransform content;
		public UI iconUI;

	}
}

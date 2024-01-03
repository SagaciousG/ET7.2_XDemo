//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIBuildMapPropertyTabComponent : Entity, IAwake, IUIComponent
	{
		public UIList modelList;
		public RectTransform lvEnableLine;
		public XToggle lvEnable;
		public RectTransform lvMaxLine;
		public XInputField lvMax;
		public RectTransform lvTypeLine;
		public XPopup lvType;
		public RectTransform groveTypeLine;
		public XPopup groveType;
		public RectTransform lvListLine;
		public UIList lvList;
		public RectTransform hpLine;
		public XInputField hp;
		public RectTransform atkLine;
		public XInputField atk;
		public RectTransform costLine;
		public XInputField cost;
		public RectTransform rateLine;
		public XInputField rate;
		public RectTransform nameLine;
		public XInputField name;
		public RectTransform content;
		public XSubUI tools;
		public UI toolsUI;

	}
}

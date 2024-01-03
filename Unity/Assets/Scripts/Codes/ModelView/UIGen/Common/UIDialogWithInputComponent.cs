//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIDialogWithInputComponent : Entity, IAwake, IUIComponent
	{
		public TextMeshProUGUI title;
		public TextMeshProUGUI desc;
		public XImage leftBtn;
		public TextMeshProUGUI leftText;
		public XImage rightBtn;
		public TextMeshProUGUI rightText;
		public XInputField input;

	}
}

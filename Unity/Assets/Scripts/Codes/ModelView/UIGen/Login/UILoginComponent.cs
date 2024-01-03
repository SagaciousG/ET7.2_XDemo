//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UILoginComponent : Entity, IAwake, IUIComponent
	{
		public TextMeshProUGUI tips;
		public XInputField account;
		public XInputField password;
		public XImage loginBtn;
		public XImage registerBtn;
		public XImage tabBtn;

	}
}

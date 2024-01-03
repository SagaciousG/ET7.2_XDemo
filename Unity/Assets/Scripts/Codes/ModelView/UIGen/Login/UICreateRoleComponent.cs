//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UICreateRoleComponent : Entity, IAwake, IUIComponent
	{
		public XImage getName;
		public TextMeshProUGUI errorText;
		public XInputField inputName;
		public XImage create;
		public XModelImage role;
		public UIList armList;
		public UIList opList;
		public UIList colorList;
		public XImage random;
		public XImage reset;
		public UIList modelList;

	}
}

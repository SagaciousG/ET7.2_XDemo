//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIBuildMapDynamicRoleTabComponent : Entity, IAwake, IUIComponent
	{
		public XToggle moveable;
		public XToggle dismoveable;
		public XImage add;
		public XImage delete;
		public XImage copy;
		public XImage paste;
		public XImage setGroup;
		public UIList groupList;
		public UIList modelList;
		public XToggle baseProperty;
		public XToggle advanceProperty;
		public RectTransform baseTab;
		public XInputField modelName;
		public XImage roleImg;
		public XPopup faction;
		public XPopup property;
		public RectTransform advanceTab;

	}
}

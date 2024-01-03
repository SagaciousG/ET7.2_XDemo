//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UISkillComponent : Entity, IAwake, IUIComponent
	{
		public UIList skillList;
		public XText name;
		public RectTransform levelUpper;
		public XText upperNum;
		public XText desc;
		public XText descNext;

	}
}

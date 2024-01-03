//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UINPCAttrComponent : Entity, IAwake, IUIComponent
	{
		public XText idNum;
		public XText coord;
		public XPopup template;
		public RectTransform idNumLine;
		public RectTransform coordLine;
		public RectTransform rotateLine;
		public XImage rotate;
		public UIList offsetList;
		public RectTransform templateLine;

	}
}

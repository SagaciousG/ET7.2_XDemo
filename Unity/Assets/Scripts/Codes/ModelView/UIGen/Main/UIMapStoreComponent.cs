//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIMapStoreComponent : Entity, IAwake, IUIComponent
	{
		public UIList mapList;
		public XImage create;
		public XImage refresh;
		public XToggle my;
		public XToggle store;
		public XImage nextPage;
		public XImage beforePage;
		public XText pageNum;

	}
}

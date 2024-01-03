//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UILobbyComponent : Entity, IAwake, IUIComponent
	{
		public XImage selectServer;
		public TextMeshProUGUI serverName;
		public XImage serverState;
		public RectTransform serverPanel;
		public XImage serverPanelBg;
		public XImage bg;
		public XImage serverPanelClose;
		public XImage tuijian;
		public UIList tabList;
		public RectTransform tuijianCont;
		public UIList lastList;
		public UIList tuijianList;
		public UIList serverList;

	}
}

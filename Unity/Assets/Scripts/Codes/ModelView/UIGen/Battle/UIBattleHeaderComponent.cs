//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIBattleHeaderComponent : Entity, IAwake, IUIComponent
	{
		public XImage state;
		public TextMeshProUGUI name;
		public XImage hpRoot;
		public XImage hp;
		public XImage mpRoot;
		public XImage mp;

	}
}

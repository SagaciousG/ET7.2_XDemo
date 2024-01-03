//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;
using System.Collections.Generic;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIBattleMainComponent : Entity, IAwake, IUIComponent
	{
		public TextMeshProUGUI timeDown;
		public TextMeshProUGUI tips;
		public XImage cancel;
		public RectTransform rightBanner;
		public RectTransform banner;
		public XImage attack;
		public XImage skillBtn;
		public XImage pet;
		public XImage item;
		public XImage auto;
		public XImage energy0;
		public XImage energy1;
		public XImage energy2;
		public XImage energy3;
		public XImage energy4;
		public XImage energy5;
		public XImage energy6;
		public XImage energy7;
		public XImage energy8;
		public XImage energy9;
		public XImage ok;
		public XImage skill0;
		public XImage skill1;
		public XImage skill2;
		public XImage skill3;
		public XImage skill4;
		public Dictionary<int, XImage> energy = new();
		public Dictionary<int, XImage> skill = new();

	}
}

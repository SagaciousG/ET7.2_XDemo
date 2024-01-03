//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using System.Collections.Generic;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UIEquipAttrContComponent : Entity, IAwake, IUIComponent
	{
		public RectTransform baseProperty;
		public RectTransform line1;
		public XText p1;
		public XText p2;
		public RectTransform line2;
		public XText p3;
		public XText p4;
		public RectTransform specialProperty;
		public XText sp1;
		public XText sp2;
		public XText sp3;
		public Dictionary<int, RectTransform> line = new();
		public Dictionary<int, XText> p = new();
		public Dictionary<int, XText> sp = new();

	}
}

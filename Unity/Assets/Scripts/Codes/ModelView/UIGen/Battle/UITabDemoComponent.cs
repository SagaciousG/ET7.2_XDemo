//自动生成文件，请勿在此编辑
using UnityEngine.UI;
using UnityEngine;
using ET;
using System.Collections.Generic;


namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public partial class UITabDemoComponent : Entity, IAwake, IUIComponent
	{
		public XUITab t1;
		public XUITab t2;
		public XUITab t3;
		public XUITab t4;
		public UI t1UI;
		public UI t2UI;
		public UI t3UI;
		public UI t4UI;
		public Dictionary<int, XUITab> t = new();

	}
}

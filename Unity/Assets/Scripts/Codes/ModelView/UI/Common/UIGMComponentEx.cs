using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ET;

namespace ET.Client
{
	public partial class UIGMComponent
	{
		public List<string> CommonUseList = new List<string>();
		public List<GMTipsItem> PopShow = new List<GMTipsItem>();
		public int SelectedId;
		public XInputField ActiveInput;
		
		public struct GMTipsItem
		{
			public string Name;
			public int Index;
		}
	}
}

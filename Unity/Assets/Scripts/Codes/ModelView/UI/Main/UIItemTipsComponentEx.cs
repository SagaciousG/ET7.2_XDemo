using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	public partial class UIItemTipsComponent
	{
		public UIBagItemArgs ShowData { get; set; }
	}

	public struct UIBagItemArgs
	{
		public int Id;
		public long Num;
		public long UID;
		public UIBagItemShowState ShowState;
	}

	public enum UIBagItemShowState
	{
		InBag,
		IsTips,
		IsEquipped,
	}
}

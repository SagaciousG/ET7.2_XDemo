using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	public partial class UIBagItemComponent
	{
		public UIBagItemArgs ShowData { get; set; }
	}

	public enum BagItemElement
	{
		Base, //包含Quality，frame，icon，num
		Selected,
		Locked,
	}

	public struct BagItemData
	{
		public List<BagItemElement> Elements;
		public BagItem Item;
		public bool Clickable;
		public Action<BagItem> CustomClick;
	}
}

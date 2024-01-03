using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	public partial class UIBagComponent
	{
		public int TabOnType { get; set; }
		public List<UIBagItemArgs> ShowList { get; set; } = new();
	}
}

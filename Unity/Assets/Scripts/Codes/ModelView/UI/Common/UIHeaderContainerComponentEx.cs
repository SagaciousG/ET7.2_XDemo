using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[Active(ActiveCode.EnterPVE)]
	public partial class UIHeaderContainerComponent: IUpdate
	{
		public List<UI> Headers = new();
		public UnitComponent UnitComponent;
	}
}

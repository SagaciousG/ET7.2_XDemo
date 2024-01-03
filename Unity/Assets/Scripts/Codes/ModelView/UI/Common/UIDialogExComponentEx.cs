using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	public partial class UIDialogExComponent
	{
		public ETTask<UIDialog.ClickedBtn> Callback { get; set; }
	}
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	public partial class UIPopTipsComponent
	{
		public int Index { get; set; } = 1;
		public Vector3 StartPos { get; set; }
		public Sequence[] Sequence { get; set; } = new Sequence[3];
	}
}

using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(Scene))]
	[Active(ActiveCode.EnterPVE)]
	public class CameraComponent : Entity, IAwake, ILateUpdate
	{
		public Unit Unit { get; set; }
		
	}
}

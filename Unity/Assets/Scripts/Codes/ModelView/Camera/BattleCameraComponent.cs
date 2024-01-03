using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(Scene))]
	public class BattleCameraComponent : Entity, IAwake<int>, ILateUpdate, IUpdate
	{
		public Camera MainCamera;
		public GameObject SelectFrame;

	}
	
}

using System;

using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(Scene))]
	[Active(ActiveCode.EnterPVE)]
	public class OperaComponent: Entity, IAwake, IUpdate, IDestroy
	{
        public Vector3 ClickPoint;

	    public int mapMask;
	    public int listenerId;
    }
}

﻿using System.Collections.Generic;

namespace ET.Client
{
	[MessageHandler(SceneType.Client)]
	public class PathfindingResultHandler : AMHandler<FindPathResultAMessage>
	{
		protected override async ETTask Run(Session session, FindPathResultAMessage message)
		{
			Unit unit = Client.UnitHelper.GetUnitFromClientScene(session.ClientScene(), message.unitId);

			float speed = unit.MoveSpeed;

			await unit.GetComponent<MoveComponent>().MoveToAsync(message.Points, speed);
		}
	}
}

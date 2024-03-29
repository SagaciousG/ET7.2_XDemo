﻿namespace ET.Client
{
	[MessageHandler(SceneType.Client)]
	public class M2C_CreateMyUnitHandler : AMHandler<CreateMyUnitAMessage>
	{
		protected override async ETTask Run(Session session, CreateMyUnitAMessage message)
		{
			// 通知场景切换协程继续往下走
			session.DomainScene().GetComponent<ObjectWait>().Notify(new Wait_CreateMyUnit() {Message = message});
			await ETTask.CompletedTask;
		}
	}
}
